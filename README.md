1. Middleware trong ASP.NET Core dùng để làm gì?
Middleware là các thành phần phần mềm được tích hợp vào pipeline (đường ống) của ứng dụng để xử lý luồng HTTP Request đi vào và HTTP Response đi ra. Các middleware thường được sử dụng để giải quyết các vấn đề chung của hệ thống (cross-cutting concerns) như:
Quản lý xác thực và phân quyền (Authentication & Authorization).
Ghi nhật ký hệ thống (Logging).
Bắt và xử lý ngoại lệ toàn cục (Exception Handling).
Quản lý bộ đệm (Caching) và cơ chế chia sẻ tài nguyên nguồn gốc chéo (CORS).
Cung cấp các tệp tĩnh (Static Files).
2. Sự khác biệt giữa Middleware và Controller
Vị trí và Mức độ: Middleware hoạt động ở mức hạ tầng HTTP của toàn bộ ứng dụng, tiếp nhận mọi request trước khi chúng được định tuyến. Controller nằm ở mức ứng dụng (Application Level), thuộc kiến trúc MVC hoặc Web API.
Chức năng: Middleware chịu trách nhiệm tiền xử lý hoặc hậu xử lý các đặc tính chung của HTTP Request/Response (ví dụ: kiểm tra header, token). Controller chịu trách nhiệm xử lý logic nghiệp vụ (Business Logic) cụ thể dựa trên endpoint mà client gọi tới, tương tác với Database và định hình cấu trúc dữ liệu trả về (JSON, XML, HTML).
3. Ý nghĩa của dòng lệnh await _next(context);
Dòng lệnh này đóng vai trò chuyển giao quyền kiểm soát chu trình. Nó truyền đối tượng HttpContext (chứa toàn bộ thông tin của request hiện tại) cho middleware tiếp theo trong pipeline để tiếp tục xử lý. Sau khi middleware tiếp theo (và các thành phần sau nó) hoàn tất, luồng thực thi sẽ quay trở lại ngay bên dưới dòng lệnh await _next(context); để thực hiện các logic hậu xử lý (nếu có).
4. Vì sao khi middleware trả về return; thì request không đi tiếp vào Controller?
Việc gọi return; mà bỏ qua _next(context) sẽ kích hoạt cơ chế đoản mạch (Short-circuiting). Khi đoản mạch xảy ra, chu trình đi tiếp (forward) của request trong pipeline bị hủy bỏ hoàn toàn. Ứng dụng sẽ ngay lập tức chuyển sang chu trình lùi (backward) để cấu trúc HTTP Response và trả về cho client. Do Controller luôn nằm ở cuối pipeline, việc ngắt mạch sớm khiến request không bao giờ có thể tiếp cận được Controller.
5. Vấn đề khi đặt middleware sau app.MapControllerRoute(...)
Pipeline trong ASP.NET Core được thực thi tuần tự theo đúng thứ tự khai báo trong mã nguồn. Các phương thức định tuyến như MapControllerRoute (hoặc MapControllers) hoạt động như một Terminal Middleware (middleware điểm cuối). Khi một request khớp với route của Controller, Controller sẽ xử lý và trả về response, đồng nghĩa với việc hoàn tất chu trình. Do đó, bất kỳ middleware nào được đăng ký phía sau dòng lệnh này sẽ bị bỏ qua và không bao giờ được thực thi đối với request đó.
6. Cách triển khai và đăng ký thêm Middleware
Việc bổ sung middleware được thực hiện tuần tự trong tệp Program.cs. Có hai phương pháp chính:
Phương pháp 1: Khai báo Inline (Inline Middleware)
Sử dụng phương thức mở rộng app.Use() trực tiếp bằng delegate:

app.Use(async (context, next) =>
{

    await next(context);

});
Phương pháp 2: Định nghĩa Class độc lập (Custom Middleware Class)
Khởi tạo một lớp (class) chứa phương thức Invoke hoặc InvokeAsync, sau đó đăng ký vào pipeline:
app.UseMiddleware<TenClassMiddlewareCuaBan>();
