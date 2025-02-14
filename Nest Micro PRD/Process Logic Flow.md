### **5.1 Məhsul Yaradılması və Stokun Yenilənməsi**

1. **Add Product (Product Service)** → Məhsul məlumatları verilənlər bazasına yazılır.
2. **Update Stock (Stock Service)** → Məhsulun stok miqdarı yenilənir.
3. **Notify (Notification Service)** → Məhsul əlavə edildikdən sonra adminə bildiriş göndərilir.

### **5.2 Sifariş Prosesi**

1. **Create Order (Order Service)** → Yeni sifariş yaradılır.
2. **Update Stock (Stock Service)** → Məhsulun stok miqdarı azalır.
3. **Payment Processing (Payment Service)** → Ödəniş həyata keçirilir.
4. **Order Status Update (Order Service)** → Sifariş statusu yenilənir.
5. **Notification (Notification Service)** → İstifadəçiyə sifariş təsdiqi göndərilir.

### **5.3 Ödəniş Uğursuzluğu Halı**

1. **Payment Failed (Payment Service)** → Ödəniş prosesi uğursuz olur.
2. **Revert Stock (Stock Service)** → Məhsulun stok miqdarı bərpa edilir.
3. **Notify (Notification Service)** → İstifadəçiyə ödəniş uğursuzluğu haqqında bildiriş göndərilir.

### **5.4 Məlumatın Analizi və Hesabatların Hazırlanması**

1. **Collect Data (Report Service)** → Məhsul, sifariş və ödəniş məlumatları toplanır.
2. **Generate Reports (Report Service)** → Statistik hesabat hazırlanır.
3. **Send Report (Notification Service)** → Hazırlanan hesabat menecerlərə göndərilir.