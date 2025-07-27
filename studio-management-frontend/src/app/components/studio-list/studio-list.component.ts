import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';

import { StudioService } from '../../services/studio.service';
import { StudioSummary } from '../../models/studio.models';
import { BookingModalComponent } from '../booking-modal/booking-modal.component';

@Component({
  selector: 'app-studio-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatAutocompleteModule
  ],
  templateUrl: './studio-list.component.html',
  styleUrl: './studio-list.component.scss'
})
export class StudioListComponent implements OnInit {
  private studioService = inject(StudioService);
  private snackBar = inject(MatSnackBar);
  private dialog = inject(MatDialog);

  studios = signal<StudioSummary[]>([]);
  loading = signal(false);
  
  selectedArea = signal('');
  selectedRadius = signal<number | null>(null);
  areas = signal<string[]>([]);
  filteredAreas = signal<string[]>([]);
  
  userLocation = signal<{ latitude: number; longitude: number } | null>(null);
  locationError = signal<string | null>(null);

  radiusOptions = [
    { value: 10, label: '10 km' },
    { value: 20, label: '20 km' }
  ];
  
  private router = inject(Router);

  ngOnInit(): void {
    this.loadStudios();
    this.getCurrentLocation();
  }

  loadStudios(): void {
    this.loading.set(true);
    
    const area = this.selectedArea();
    const radius = this.selectedRadius();
    const location = this.userLocation();

    if (radius && location) {
      this.studioService.getNearbyStudios(location.latitude, location.longitude, radius).subscribe({
        next: (studios: StudioSummary[]) => {
          this.studios.set(studios);
          this.extractAreasFromStudios(studios);
          this.loading.set(false);
        },
        error: (error: any) => {
          console.error('Error loading nearby studios:', error);
          this.snackBar.open('Error loading nearby studios', 'Close', { duration: 3000 });
          this.loading.set(false);
        }
      });
    } else if (area) {
      this.studioService.searchStudiosByArea(area).subscribe({
        next: (studios: StudioSummary[]) => {
          this.studios.set(studios);
          this.extractAreasFromStudios(studios);
          this.loading.set(false);
        },
        error: (error: any) => {
          console.error('Error loading studios by area:', error);
          this.snackBar.open('Error loading studios', 'Close', { duration: 3000 });
          this.loading.set(false);
        }
      });
    } else {
      this.studioService.getStudios().subscribe({
        next: (studios: StudioSummary[]) => {
          this.studios.set(studios);
          this.extractAreasFromStudios(studios);
          this.loading.set(false);
        },
        error: (error: any) => {
          console.error('Error loading all studios:', error);
          this.snackBar.open('Error loading studios', 'Close', { duration: 3000 });
          this.loading.set(false);
        }
      });
    }
  }

  private extractAreasFromStudios(studios: StudioSummary[]): void {
    const uniqueAreas = [...new Set(studios.map(s => s.area))].sort();
    this.areas.set(uniqueAreas);
    this.filteredAreas.set(uniqueAreas);
  }

  private getCurrentLocation(): void {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          this.userLocation.set({
            latitude: position.coords.latitude,
            longitude: position.coords.longitude
          });
          this.locationError.set(null);
        },
        (error) => {
          let errorMessage = 'Unable to get your location';
          switch (error.code) {
            case error.PERMISSION_DENIED:
              errorMessage = 'Location access denied by user';
              break;
            case error.POSITION_UNAVAILABLE:
              errorMessage = 'Location information unavailable';
              break;
            case error.TIMEOUT:
              errorMessage = 'Location request timed out';
              break;
          }
          this.locationError.set(errorMessage);
          this.snackBar.open(errorMessage, 'Close', { duration: 5000 });
        }
      );
    } else {
      this.locationError.set('Geolocation is not supported by this browser');
    }
  }

  onAreaChange(): void {
    this.selectedRadius.set(null);
    this.loadStudios();
  }

  onRadiusChange(): void {
    if (this.userLocation()) {
      this.selectedArea.set('');
      this.loadStudios();
    } else {
      this.snackBar.open('Location not available for radius search', 'Close', { duration: 3000 });
      this.selectedRadius.set(null);
    }
  }

  filterAreas(query: string): void {
    const filterValue = query.toLowerCase();
    this.filteredAreas.set(
      this.areas().filter(area => area.toLowerCase().includes(filterValue))
    );
  }

  clearFilters(): void {
    this.selectedArea.set('');
    this.selectedRadius.set(null);
    this.loadStudios();
  }

  openBookingModal(studio: StudioSummary): void {
    const dialogRef = this.dialog.open(BookingModalComponent, {
      data: { studio },
      width: '600px',
      maxHeight: '90vh'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result?.success) {
        this.snackBar.open('Booking created successfully!', 'Close', { duration: 3000 });
      }
    });
  }

  getStarArray(rating: number): number[] {
    return Array(5).fill(0).map((_, i) => i < Math.floor(rating) ? 1 : 0);
  }

  getStudioImage(studio: StudioSummary): string {
    return studio.images && studio.images.length > 0 
      ? studio.images[0] 
      : '/assets/images/default-studio.jpg';
  }
  
  goBack(): void {
    this.router.navigate(['/dashboard']);
  }
} 