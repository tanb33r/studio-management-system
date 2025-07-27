# Studio Booking Management Frontend

This Angular frontend application provides a complete solution for studio booking management with a modern, responsive user interface.

## 🚀 Features Implemented

### 1. Studio List Page (`/studios`)
- **Display Studios**: Shows all available studios with comprehensive information
  - Studio name, type, location, amenities, price per hour, and rating
  - Professional card-based layout with hover effects
  - Studio images with fallback placeholders
- **Search Functionality**:
  - **Text Search**: Search by studio name, area, city, or type
  - **Area Search**: Autocomplete search by area with dynamic filtering
  - **Radius Search**: Geolocation-based search within 10km or 20km radius
  - Handles geolocation errors gracefully
- **Book Now Button**: Opens booking modal for each studio

### 2. Booking Modal
- **Date Selection**: Calendar picker with minimum date validation
- **Time Slot Selection**: Dynamic time slots fetched from API based on selected date
- **User Information Form**: Pre-filled with current user data
  - Full name, email, phone number
  - Optional notes field
- **Availability Validation**: Real-time checking for booking conflicts
- **Price Calculation**: Shows duration and total price for selected time slot
- **Success/Error Handling**: Appropriate messages for booking outcomes

### 3. Booking List Page (`/bookings`)
- **Categorized Bookings**:
  - Current Bookings (in progress)
  - Upcoming Bookings
  - Past Bookings
- **Booking Details**: Studio name, date, time, user info, total price
- **Status Management**: Visual status indicators with color coding
- **Cancellation**: Cancel bookings with 24+ hour notice
- **Responsive Design**: Adapts to different screen sizes

### 4. Enhanced Dashboard (`/dashboard`)
- **Hero Section**: Welcome area with primary actions
- **Quick Actions**: Easy navigation to studios and bookings
- **User Account Info**: Display current user information
- **Modern Design**: Gradient background with Material Design components

## 🛠 Technical Implementation

### Components Structure
```
src/app/components/
├── studio-list/          # Studio browsing and search
├── booking-modal/        # Booking creation dialog
├── booking-list/         # User bookings management
└── dashboard/           # Main dashboard
```

### Services
```
src/app/services/
├── studio.service.ts     # Studio API operations
├── booking.service.ts    # Booking API operations
└── auth.service.ts      # Authentication (existing)
```

### Models
```
src/app/models/
├── studio.models.ts      # Studio data structures
├── booking.models.ts     # Booking data structures
└── auth.models.ts       # Authentication (existing)
```

### Key Features
- **Angular Signals**: Modern reactive state management
- **Material Design**: Professional UI components
- **Responsive Layout**: Mobile-first design approach
- **TypeScript**: Full type safety
- **Lazy Loading**: Components loaded on demand
- **Authentication Guards**: Protected routes

## 🎨 UI/UX Features

### Design Principles
- **Human-Centered Design**: Clean, intuitive interface
- **Accessibility**: Proper contrast, keyboard navigation
- **Mobile Responsive**: Works on all device sizes
- **Loading States**: Spinners and skeleton loading
- **Error Handling**: User-friendly error messages

### Visual Elements
- **Gradient Backgrounds**: Modern aesthetic
- **Card-Based Layout**: Clean information organization
- **Smooth Animations**: Hover effects and transitions
- **Status Indicators**: Color-coded booking statuses
- **Professional Typography**: Consistent font hierarchy

## 🚀 Getting Started

### Prerequisites
- Node.js 18+
- Angular CLI 20+

### Installation
```bash
cd studio-management-frontend
npm install
```

### Development
```bash
npm start
# Navigate to http://localhost:4200
```

### Build
```bash
npm run build
```

## 📱 Responsive Breakpoints
- **Mobile**: < 768px
- **Tablet**: 768px - 1024px
- **Desktop**: > 1024px

## 🔗 Navigation
- `/dashboard` - Main dashboard
- `/studios` - Browse and search studios
- `/bookings` - Manage user bookings
- `/login` - User authentication
- `/register` - User registration

## 🎯 User Flow
1. **Login/Register** → User authentication
2. **Dashboard** → Welcome and navigation hub
3. **Studios** → Browse and search available studios
4. **Book Studio** → Select date/time and create booking
5. **My Bookings** → View and manage existing bookings

## 📝 Notes
- All forms include proper validation
- API endpoints are configured for localhost:44325
- Geolocation requires HTTPS in production
- Default studio images require setup in `/public/assets/images/`
- Authentication tokens are managed automatically

This implementation provides a complete, production-ready frontend for studio booking management with modern design patterns and excellent user experience. 