-- Seed data for motoklub-bezbednost database

-- MemberType seed data
INSERT INTO MemberType (id, prefix, typeName, color, paidMembership) VALUES
(1, 1, 'Uprava', '#00BCD4', 1),
(2, 2, 'Aktivni - pridruženi članovi (redovni)', '#4CAF50', 1),
(3, 3, 'Deca i prijatelji kluba', '#FFEB3B', 0),
(4, 4, 'Počasni članovi i 65+', '#9C27B0', 0);

-- PaymentType seed data
INSERT INTO PaymentType (id, type, description) VALUES
(1, 'Keš', 'Gotovinsko plaćanje'),
(2, 'EBanking', 'Plaćanje elektronskim bankarstvom'),
(3, 'Faktura', 'Plaćanje preko fakture');

-- Level seed data
INSERT INTO Level (id, name, note) VALUES
(1, 'Osnovni', 'Osnovni nivo treninga'),
(2, 'Napredni', 'Napredni nivo treninga'),
(3, 'OffRoad', 'Offroad vožnja');

