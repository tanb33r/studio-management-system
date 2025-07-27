import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';

import { StudioService } from '../../services/studio.service';
import { BookingService } from '../../services/booking.service';
import { AuthService } from '../../services/auth.service';
import { StudioSummary, StudioAvailability, TimeSlot } from '../../models/studio.models';
import { CreateBookingRequest } from '../../models/booking.models';

interface DialogData {
  studio: StudioSummary;
}

@Component({
  selector: 'app-booking-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatIconModule
  ],
  templateUrl: './booking-modal.component.html',
  styleUrl: './booking-modal.component.scss'
})
export class BookingModalComponent implements OnInit {
  private dialogRef = inject(MatDialogRef<BookingModalComponent>);
  private data = inject<DialogData>(MAT_DIALOG_DATA);
  private fb = inject(FormBuilder);
  private studioService = inject(StudioService);
  private bookingService = inject(BookingService);
  private authService = inject(AuthService);
  private snackBar = inject(MatSnackBar);

  studio = this.data.studio;
  bookingForm: FormGroup;
  
  loadingSlots = signal(false);
  submitting = signal(false);
  availability = signal<StudioAvailability | null>(null);
  selectedDate = signal<Date | null>(null);
  
  availableSlots = computed(() => {
    const avail = this.availability();
    return avail ? avail.availableSlots.filter(slot => slot.isAvailable) : [];
  });

  constructor() {
    const currentUser = this.authService.getCurrentUser();
    
    this.bookingForm = this.fb.group({
      date: ['', Validators.required],
      timeSlot: ['', Validators.required],
      userName: [currentUser?.firstName + ' ' + currentUser?.lastName || '', Validators.required],
      email: [currentUser?.email || '', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      notes: ['']
    });
  }

  ngOnInit(): void {
    const today = new Date();
    this.bookingForm.patchValue({
      date: today
    });
  }

  get minDate(): Date {
    return new Date();
  }

  getStarArray(rating: number): number[] {
    return Array(5).fill(0).map((_, i) => i < Math.floor(rating) ? 1 : 0);
  }

  onDateChange(date: Date): void {
    if (!date) return;
    
    this.selectedDate.set(date);
    this.loadingSlots.set(true);
    
    const dateString = this.formatDate(date);
    
    this.studioService.getStudioAvailability(this.studio.id, dateString).subscribe({
      next: (availability) => {
        this.availability.set(availability);
        this.loadingSlots.set(false);
        this.bookingForm.patchValue({ timeSlot: '' });
      },
      error: (error) => {
        console.error('Error loading availability:', error);
        this.snackBar.open('Error loading available time slots', 'Close', { duration: 3000 });
        this.loadingSlots.set(false);
      }
    });
  }

  formatTimeSlot(slot: TimeSlot): string {
    return `${this.formatTime(slot.startTime)} - ${this.formatTime(slot.endTime)}`;
  }

  private formatTime(time: string): string {
    const [hours, minutes] = time.split(':');
    const hour = parseInt(hours);
    const ampm = hour >= 12 ? 'PM' : 'AM';
    const displayHour = hour % 12 || 12;
    return `${displayHour}:${minutes} ${ampm}`;
  }

  private formatDate(date: Date): string {
    return date.toISOString().split('T')[0];
  }

  onSubmit(): void {
    if (this.bookingForm.invalid) {
      this.markFormGroupTouched();
      return;
    }

    this.submitting.set(true);
    
    const formValue = this.bookingForm.value;
    const selectedSlot = this.availableSlots().find(slot => 
      this.formatTimeSlot(slot) === formValue.timeSlot
    );

    if (!selectedSlot) {
      this.snackBar.open('Invalid time slot selected', 'Close', { duration: 3000 });
      this.submitting.set(false);
      return;
    }

    const request: CreateBookingRequest = {
      studioId: this.studio.id,
      userName: formValue.userName,
      email: formValue.email,
      phone: formValue.phone,
      date: this.formatDate(formValue.date),
      startTime: selectedSlot.startTime,
      endTime: selectedSlot.endTime,
      notes: formValue.notes || ''
    };

    this.bookingService.checkAvailability({
      studioId: request.studioId,
      date: request.date,
      startTime: request.startTime,
      endTime: request.endTime
    }).subscribe({
      next: (conflictResponse) => {
        if (conflictResponse.hasConflict) {
          this.snackBar.open(conflictResponse.message, 'Close', { duration: 5000 });
          this.submitting.set(false);
        } else {
          this.createBooking(request);
        }
      },
      error: (error) => {
        console.error('Error checking availability:', error);
        this.snackBar.open('Error checking availability', 'Close', { duration: 3000 });
        this.submitting.set(false);
      }
    });
  }

  private createBooking(request: CreateBookingRequest): void {
    this.bookingService.createBooking(request).subscribe({
      next: (confirmation) => {
        this.submitting.set(false);
        this.dialogRef.close({ success: true, booking: confirmation });
      },
      error: (error) => {
        console.error('Error creating booking:', error);
        this.submitting.set(false);
        
        let errorMessage = 'Error creating booking';
        if (error.error?.message) {
          errorMessage = error.error.message;
        }
        
        this.snackBar.open(errorMessage, 'Close', { duration: 5000 });
      }
    });
  }

  private markFormGroupTouched(): void {
    Object.keys(this.bookingForm.controls).forEach(key => {
      const control = this.bookingForm.get(key);
      control?.markAsTouched();
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  getFieldError(fieldName: string): string {
    const field = this.bookingForm.get(fieldName);
    if (field?.errors && field?.touched) {
      if (field.errors['required']) return `${fieldName} is required`;
      if (field.errors['email']) return 'Invalid email format';
    }
    return '';
  }

  calculateDuration(slot: TimeSlot): number {
    const start = new Date(`2000-01-01T${slot.startTime}`);
    const end = new Date(`2000-01-01T${slot.endTime}`);
    return (end.getTime() - start.getTime()) / (1000 * 60 * 60); // hours
  }

  calculatePrice(slot: TimeSlot): number {
    const duration = this.calculateDuration(slot);
    return duration * this.studio.pricePerHour;
  }
} 