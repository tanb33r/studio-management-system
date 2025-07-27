-- Studio Booking Management - Seed Data Script
-- Based on mock data from: https://gist.github.com/rash3dul-islam/88e1565bea2dd1ff9180ff733617a565

USE [StudioManagementDB]
GO

-- Clear existing data (if any)
DELETE FROM [Bookings];
DELETE FROM [Studios];

-- Reset identity seeds
DBCC CHECKIDENT ('Studios', RESEED, 0);
DBCC CHECKIDENT ('Bookings', RESEED, 0);

-- Insert Studio Data
INSERT INTO [Studios] 
(Name, Description, Area, Address, City, State, ZipCode, Country, Latitude, Longitude, PricePerHour, Currency, Capacity, ContactPhone, ContactEmail, StudioType, Amenities, Equipment, Images, OpeningHours, IsActive, OwnerId, Rating, ReviewCount)
VALUES
-- Studio 1
('Dhanmondi Recording Studio 1', 'A popular Recording Studio studio in Gulshan area.', 'Gulshan', 'House 73, Road 9, Mohammadpur, Dhaka', 'Dhaka', 'Dhaka Division', '1207', 'Bangladesh', 23.716008, 90.410068, 1525.00, 'BDT', 10, '+8801768421018', 'studio1@example.com', 'Photography', 'Instruments,Lighting Equipment,Wi-Fi', '', 'https://example.com/images/studio1_1.jpg,https://example.com/images/studio1_2.jpg', '{"open":"09:00","close":"18:00"}', 1, 'owner1', 4.0, 15),

-- Studio 2
('Bashundhara Photography 2', 'A popular Rehearsal Space studio in Dhanmondi area.', 'Gulshan', 'House 70, Road 9, Bashundhara, Dhaka', 'Dhaka', 'Dhaka Division', '1229', 'Bangladesh', 23.793419, 90.494277, 2650.00, 'BDT', 12, '+8801619267658', 'studio2@example.com', 'Recording Studio', 'Backdrop Options,Soundproofing,Changing Room,Makeup Room,High-end Microphones', '', 'https://example.com/images/studio2_1.jpg,https://example.com/images/studio2_2.jpg', '{"open":"11:00","close":"21:00"}', 1, 'owner2', 4.3, 28),

-- Studio 3
('Mirpur Rehearsal Space 3', 'A popular Art Studio studio in Mirpur area.', 'Badda', 'House 37, Road 1, Shyamoli, Dhaka', 'Dhaka', 'Dhaka Division', '1207', 'Bangladesh', 23.837292, 90.313015, 2216.00, 'BDT', 8, '+8801544532370', 'studio3@example.com', 'Recording Studio', 'Backdrop Options,Mixing Console,Props', '', 'https://example.com/images/studio3_1.jpg,https://example.com/images/studio3_2.jpg', '{"open":"08:00","close":"18:00"}', 1, 'owner3', 4.2, 22),

-- Studio 4
('Uttara Art Studio 4', 'A popular Photography studio in Uttara area.', 'Bashundhara', 'House 69, Road 1, Banani, Dhaka', 'Dhaka', 'Dhaka Division', '1213', 'Bangladesh', 23.822039, 90.343635, 1520.00, 'BDT', 15, '+8801398939207', 'studio4@example.com', 'Rehearsal Space', 'Backdrop Options,High-end Microphones,Soundproofing,Changing Room', '', 'https://example.com/images/studio4_1.jpg,https://example.com/images/studio4_2.jpg', '{"open":"11:00","close":"22:00"}', 1, 'owner4', 4.2, 35),

-- Studio 5
('Uttara Recording Studio 5', 'A popular Recording Studio studio in Bashundhara area.', 'Bashundhara', 'House 10, Road 7, Uttara, Dhaka', 'Dhaka', 'Dhaka Division', '1230', 'Bangladesh', 23.757238, 90.385777, 2100.00, 'BDT', 6, '+8801234567890', 'studio5@example.com', 'Recording Studio', 'High-end Microphones,Soundproofing,Mixing Console', '', 'https://example.com/images/studio5_1.jpg,https://example.com/images/studio5_2.jpg', '{"open":"10:00","close":"20:00"}', 1, 'owner5', 4.5, 42),

-- Studio 6
('Dhanmondi Photography Studio 6', 'Professional photography studio in Dhanmondi.', 'Dhanmondi', 'House 15, Road 3, Dhanmondi, Dhaka', 'Dhaka', 'Dhaka Division', '1205', 'Bangladesh', 23.746466, 90.376015, 1800.00, 'BDT', 20, '+8801987654321', 'studio6@example.com', 'Photography', 'Professional Lighting,Backdrop Options,Changing Room,Props', '', 'https://example.com/images/studio6_1.jpg,https://example.com/images/studio6_2.jpg', '{"open":"09:00","close":"21:00"}', 1, 'owner6', 4.1, 18),

-- Studio 7
('Gulshan Music Studio 7', 'State-of-the-art music recording studio.', 'Gulshan', 'House 45, Road 11, Gulshan, Dhaka', 'Dhaka', 'Dhaka Division', '1212', 'Bangladesh', 23.781174, 90.417458, 3200.00, 'BDT', 8, '+8801876543210', 'studio7@example.com', 'Recording Studio', 'High-end Microphones,Soundproofing,Mixing Console,Instruments', '', 'https://example.com/images/studio7_1.jpg,https://example.com/images/studio7_2.jpg', '{"open":"12:00","close":"24:00"}', 1, 'owner7', 4.8, 67),

-- Studio 8
('Banani Art Studio 8', 'Creative art studio for artists and workshops.', 'Banani', 'House 22, Road 8, Banani, Dhaka', 'Dhaka', 'Dhaka Division', '1213', 'Bangladesh', 23.794192, 90.404542, 1400.00, 'BDT', 25, '+8801765432109', 'studio8@example.com', 'Art Studio', 'Natural Lighting,Art Supplies,Wi-Fi,Tables,Chairs', '', 'https://example.com/images/studio8_1.jpg,https://example.com/images/studio8_2.jpg', '{"open":"08:00","close":"18:00"}', 1, 'owner8', 3.9, 12),

-- Studio 9
('Mohammadpur Dance Studio 9', 'Spacious dance rehearsal studio.', 'Mohammadpur', 'House 88, Road 2, Mohammadpur, Dhaka', 'Dhaka', 'Dhaka Division', '1207', 'Bangladesh', 23.761978, 90.356715, 1200.00, 'BDT', 30, '+8801654321098', 'studio9@example.com', 'Rehearsal Space', 'Mirrors,Sound System,Wooden Floor,Air Conditioning', '', 'https://example.com/images/studio9_1.jpg,https://example.com/images/studio9_2.jpg', '{"open":"06:00","close":"22:00"}', 1, 'owner9', 4.4, 31),

-- Studio 10
('Bashundhara Event Studio 10', 'Large event and photo studio.', 'Bashundhara', 'House 12, Road 5, Bashundhara, Dhaka', 'Dhaka', 'Dhaka Division', '1229', 'Bangladesh', 23.825154, 90.425789, 2800.00, 'BDT', 50, '+8801543210987', 'studio10@example.com', 'Photography', 'Professional Lighting,Backdrop Options,Changing Room,Makeup Room,Props', '', 'https://example.com/images/studio10_1.jpg,https://example.com/images/studio10_2.jpg', '{"open":"10:00","close":"23:00"}', 1, 'owner10', 4.6, 89);

-- Add more sample bookings for testing
INSERT INTO [Bookings] 
(StudioId, UserName, Email, Phone, Date, StartTime, EndTime, DurationHours, TotalPrice, Currency, Status, Notes, BookingReference)
VALUES
(1, 'John Doe', 'john.doe@email.com', '+8801712345678', '2025-01-28', '10:00:00', '14:00:00', 4, 6100.00, 'BDT', 1, 'Photography session for wedding', 'BK20250127ABC123'),
(2, 'Jane Smith', 'jane.smith@email.com', '+8801687654321', '2025-01-29', '15:00:00', '18:00:00', 3, 7950.00, 'BDT', 1, 'Music recording session', 'BK20250127DEF456'),
(3, 'Mike Johnson', 'mike.johnson@email.com', '+8801798765432', '2025-01-30', '09:00:00', '12:00:00', 3, 6648.00, 'BDT', 0, 'Band practice session', 'BK20250127GHI789'),
(4, 'Sarah Wilson', 'sarah.wilson@email.com', '+8801765432198', '2025-01-31', '16:00:00', '20:00:00', 4, 6080.00, 'BDT', 1, 'Dance rehearsal', 'BK20250127JKL012'),
(5, 'David Brown', 'david.brown@email.com', '+8801654321987', '2025-02-01', '11:00:00', '15:00:00', 4, 8400.00, 'BDT', 0, 'Album recording', 'BK20250127MNO345');

PRINT 'Seed data inserted successfully!';
PRINT 'Studios inserted: 10';
PRINT 'Sample bookings inserted: 5'; 