import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';

import { BookingService } from '../../services/booking.service';
import { Booking, BookingStatus } from '../../models/booking.models';

@Component({
  selector: 'app-booking-list',
  standalone: true,
  imports: [
    CommonModule,
    MatListModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './booking-list.component.html',
  styleUrl: './booking-list.component.scss'
})
export class BookingListComponent implements OnInit {
  private bookingService = inject(BookingService);
  private snackBar = inject(MatSnackBar);

  bookings = signal<Booking[]>([]);
  loading = signal(false);
  
  BookingStatus = BookingStatus;
  private router = inject(Router);

  ngOnInit(): void {
    this.loadBookings();
  }

  private loadBookings(): void {
    this.loading.set(true);
    
    this.bookingService.getAllBookings().subscribe({
      next: (bookings: Booking[]) => {
        this.bookings.set(bookings);
        this.loading.set(false);
        // console.log(bookings)
      },
      error: (error: any) => {
        console.error('Error loading bookings:', error);
        this.snackBar.open('Error loading bookings', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  getStatusText(status: BookingStatus): string {
    switch (status) {
      case BookingStatus.Pending: return 'Pending';
      case BookingStatus.Confirmed: return 'Confirmed';
      case BookingStatus.Cancelled: return 'Cancelled';
      case BookingStatus.Completed: return 'Completed';
      default: return 'Unknown';
    }
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString();
  }

  formatTime(timeString: string): string {
    const [hours, minutes] = timeString.split(':');
    const hour = parseInt(hours);
    const ampm = hour >= 12 ? 'PM' : 'AM';
    const displayHour = hour % 12 || 12;
    return `${displayHour}:${minutes} ${ampm}`;
  }
  
  goBack(): void {
    this.router.navigate(['/dashboard']);
  }
} 