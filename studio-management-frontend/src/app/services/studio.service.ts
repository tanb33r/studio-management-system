import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Studio, StudioSummary, StudioAvailability } from '../models/studio.models';
import { API_CONFIG, getApiUrl } from '../config/api.config';

@Injectable({
  providedIn: 'root'
})
export class StudioService {
  private http = inject(HttpClient);
  private apiUrl = getApiUrl(API_CONFIG.ENDPOINTS.STUDIOS);

  getStudios(): Observable<StudioSummary[]> {
    return this.http.get<StudioSummary[]>(this.apiUrl);
  }

  getStudioById(id: number): Observable<Studio> {
    return this.http.get<Studio>(`${this.apiUrl}/${id}`);
  }

  getStudioAvailability(studioId: number, date: string): Observable<StudioAvailability> {
    const params = new HttpParams().set('date', date);
    return this.http.get<StudioAvailability>(`${this.apiUrl}/${studioId}/availability`, { params });
  }

  searchStudiosByArea(area?: string): Observable<StudioSummary[]> {
    let params = new HttpParams();
    
    if (area) {
      params = params.set('area', area);
    }

    return this.http.get<StudioSummary[]>(`${this.apiUrl}/search`, { params });
  }

  getNearbyStudios(latitude: number, longitude: number, radiusKm: number = 10): Observable<StudioSummary[]> {
    const params = new HttpParams()
      .set('lat', latitude.toString())
      .set('lng', longitude.toString())
      .set('radius', radiusKm.toString());
    
    return this.http.get<StudioSummary[]>(`${this.apiUrl}/nearby`, { params });
  }
} 