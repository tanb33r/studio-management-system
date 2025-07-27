export enum BookingStatus {
  Pending = 0,
  Confirmed = 1,
  Cancelled = 2,
  Completed = 3
}

export interface Booking {
  id: number;
  studioId: number;
  studioName: string;
  userName: string;
  email: string;
  phone: string;
  date: string;
  startTime: string;
  endTime: string;
  durationHours: number;
  totalPrice: number;
  currency: string;
  status: BookingStatus;
  notes: string;
  bookingReference: string;
  createdAt: string;
  confirmedAt?: string;
  cancelledAt?: string;
  cancellationReason?: string;
}

export interface BookingSummary {
  id: number;
  studioName: string;
  userName: string;
  date: string;
  startTime: string;
  endTime: string;
  status: BookingStatus;
  bookingReference: string;
  totalPrice: number;
  currency: string;
}

export interface CreateBookingRequest {
  studioId: number;
  userName: string;
  email: string;
  phone: string;
  date: string;
  startTime: string;
  endTime: string;
  notes: string;
}

export interface UpdateBookingRequest {
  date: string;
  startTime: string;
  endTime: string;
  notes: string;
}

export interface BookingConflictResponse {
  hasConflict: boolean;
  message: string;
  conflictingBookings: ConflictingBooking[];
}

export interface ConflictingBooking {
  id: number;
  date: string;
  startTime: string;
  endTime: string;
  bookingReference: string;
  status: BookingStatus;
}

export interface BookingAvailabilityRequest {
  studioId: number;
  date: string;
  startTime: string;
  endTime: string;
}

export interface BookingConfirmation {
  bookingId: number;
  bookingReference: string;
  isConfirmed: boolean;
  message: string;
  confirmedAt?: string;
} 