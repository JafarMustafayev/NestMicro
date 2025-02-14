### **3.1 Mikroxidmətlər**

1. **Auth Service:** İstifadəçi qeydiyyatı, avtorizasiya, identifikasiya.
2. **Product Service:** Məhsul məlumatlarının idarə edilməsi.
3. **Stock Service:** Məhsul ehtiyatlarının idarə edilməsi.
4. **Order Service:** Sifarişlərin idarə edilməsi və status izlənməsi.
5. **Payment Service:** Ödəniş əməliyyatlarının idarə edilməsi.
6. **Notification Service:** Bildirişlərin göndərilməsi (e-poçt, SMS).
7. **Storage Service:** Faylların yüklənməsi və saxlanılması.
8. **Review Service:** İstifadəçi rəylərinin idarə edilməsi.
9. **Report Service:** Statistik məlumatların emalı və faktura hazırlanması.

---

### **3.2 Mikroxidmətlərarası Əlaqə**

- **Message Broker:** MassTransit RabbitMQ istifadə ediləcək.
- **Kommunikasiya:** Pub-Sub və Queue modelləri tətbiq ediləcək.
- **API Gateway:** Bütün xidmətlərə vahid giriş nöqtəsi təmin ediləcək.

---

### **3.3 Authentication və Authorization**

- **JWT (JSON Web Token)** istifadə ediləcək.
- RBAC (Role-Based Access Control) tətbiq ediləcək.