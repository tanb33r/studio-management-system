import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { StudioService } from '../../services/studio.service';
import { Studio } from '../../models/studio.models';
import { BookingModalComponent } from '../booking-modal/booking-modal.component';

@Component({
  selector: 'app-studio-details',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './studio-details.component.html',
  styleUrl: './studio-details.component.scss'
})
export class StudioDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private studioService = inject(StudioService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  studio = signal<Studio | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadStudio(parseInt(id));
    } else {
      this.error.set('Invalid studio ID');
    }
  }

  private loadStudio(id: number): void {
    this.loading.set(true);
    this.error.set(null);

    this.studioService.getStudioById(id).subscribe({
      next: (studio: Studio) => {
        this.studio.set(studio);
        this.loading.set(false);
      },
      error: (error: any) => {
        console.error('Error loading studio:', error);
        this.error.set('Studio not found');
        this.loading.set(false);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/studios']);
  }

  openBookingModal(): void {
    const studio = this.studio();
    if (!studio) return;

    const studioSummary = {
      id: studio.id,
      name: studio.name,
      area: studio.area,
      city: studio.city,
      state: studio.state,
      pricePerHour: studio.pricePerHour,
      currency: studio.currency,
      studioType: studio.studioType,
      rating: studio.rating,
      reviewCount: studio.reviewCount,
      images: studio.images
    };

    const dialogRef = this.dialog.open(BookingModalComponent, {
      data: { studio: studioSummary },
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

  getMainImage(): string {
    const studio = this.studio();
    return studio?.images && studio.images.length > 0 
      ? studio.images[0] 
      : '/assets/images/default-studio.jpg';
  }

  formatOpeningHours(hours: string): string {
    if (!hours) return 'Hours not specified';
    
    try {
      const hoursObj = JSON.parse(hours);
      
      if (hoursObj.open && hoursObj.close) {
        const openTime = this.formatTime(hoursObj.open);
        const closeTime = this.formatTime(hoursObj.close);
        return `${openTime} - ${closeTime}`;
      }
      
      return hours; 
    } catch (error) {
      return hours;
    }
  }

  private formatTime(timeString: string): string {
    if (!timeString) return '';
    
    const [hours, minutes] = timeString.split(':');
    const hour = parseInt(hours);
    const ampm = hour >= 12 ? 'PM' : 'AM';
    const displayHour = hour % 12 || 12;
    return `${displayHour}:${minutes} ${ampm}`;
  }
} 