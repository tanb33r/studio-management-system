import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { 
  Booking, 
  CreateBookingRequest, 
  UpdateBookingRequest,
  BookingAvailabilityRequest,
  BookingConflictResponse,
  BookingConfirmation
} from '../models/booking.models';
import { API_CONFIG, getApiUrl } from '../config/api.config';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private http = inject(HttpClient);
  private apiUrl = getApiUrl(API_CONFIG.ENDPOINTS.BOOKINGS);

  getAllBookings(): Observable<Booking[]> {
    return this.http.get<Booking[]>(this.apiUrl);
  }

  getBookingById(id: number): Observable<Booking> {
    return this.http.get<Booking>(`${this.apiUrl}/${id}`);
  }

  getBookingByReference(reference: string): Observable<Booking> {
    return this.http.get<Booking>(`${this.apiUrl}/reference/${reference}`);
  }

  getBookingsByUser(email: string): Observable<Booking[]> {
    return this.http.get<Booking[]>(`${this.apiUrl}/user/${email}`);
  }

  getBookingsByStudio(studioId: number): Observable<Booking[]> {
    return this.http.get<Booking[]>(`${this.apiUrl}/studio/${studioId}`);
  }

  checkAvailability(request: BookingAvailabilityRequest): Observable<BookingConflictResponse> {
    return this.http.post<BookingConflictResponse>(`${this.apiUrl}/check-availability`, request);
  }

  createBooking(request: CreateBookingRequest): Observable<Booking> {
    return this.http.post<Booking>(this.apiUrl, request);
  }

  updateBooking(id: number, request: UpdateBookingRequest): Observable<Booking> {
    return this.http.put<Booking>(`${this.apiUrl}/${id}`, request);
  }

  confirmBooking(id: number): Observable<BookingConfirmation> {
    return this.http.post<BookingConfirmation>(`${this.apiUrl}/${id}/confirm`, {});
  }

  cancelBooking(id: number, reason?: string): Observable<Booking> {
    const body = reason ? { reason } : {};
    return this.http.post<Booking>(`${this.apiUrl}/${id}/cancel`, body);
  }

  deleteBooking(id: number): Observable<{ message: string }> {
    return this.http.delete<{ message: string }>(`${this.apiUrl}/${id}`);
  }
} 