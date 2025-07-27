export interface Studio {
  id: number;
  name: string;
  description: string;
  area: string;
  address: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
  latitude: number;
  longitude: number;
  pricePerHour: number;
  currency: string;
  capacity: number;
  contactPhone: string;
  contactEmail: string;
  studioType: string;
  amenities: string[];
  equipment: string[];
  images: string[];
  openingHours: string;
  isActive: boolean;
  rating: number;
  reviewCount: number;
  createdAt: string;
  updatedAt?: string;
}

export interface StudioSummary {
  id: number;
  name: string;
  area: string;
  city: string;
  state: string;
  pricePerHour: number;
  currency: string;
  studioType: string;
  rating: number;
  reviewCount: number;
  images: string[];
  distanceKm?: number;
}

export interface CreateStudioRequest {
  name: string;
  description: string;
  area: string;
  address: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
  latitude: number;
  longitude: number;
  pricePerHour: number;
  capacity: number;
  contactPhone: string;
  contactEmail: string;
  studioType: string;
  amenities: string[];
  equipment: string[];
  images: string[];
  openingHours: string;
}

export interface StudioAvailability {
  studioId: number;
  studioName: string;
  date: string;
  availableSlots: TimeSlot[];
  bookedSlots: TimeSlot[];
}

export interface TimeSlot {
  startTime: string;
  endTime: string;
  isAvailable: boolean;
  bookingReference?: string;
}

 