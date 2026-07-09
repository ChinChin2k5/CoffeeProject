using CoffeeShop.DAL.Repositories;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
public class StaffService
{
    private readonly StaffDAL _staffDal;
    public StaffService (StaffDAL staffDal)
    {
        _staffDal = staffDal;
    }
    public async Task<StaffResponse> CreateNewImportAsync (StaffRequest request)
    {
        // 1. Xác thực Staff (Kiểm tra nhân viên có tồn tại không)
            var staff = await _staffDal.GetUserByIdAsync(request.StaffId);
            if (staff == null) 
                throw new Exception("Lỗi: Không tìm thấy nhân viên!");

            // 2. Lấy thông tin Tồn kho hiện tại dựa vào ItemId
            var inventory = await _staffDal.GetInventoryByIdAsync(request.ItemId);
            if (inventory == null) 
                throw new Exception("Lỗi: Sản phẩm chưa được khởi tạo trong kho!");

            // 3. LẤY TÊN SẢN PHẨM TỪ BẢNG PRODUCT (Chìa khóa giải quyết bài toán của em)
            var product = await _staffDal.GetProductByIdAsync(request.ItemId);
            if (product == null) 
                throw new Exception("Lỗi: ID Sản phẩm không tồn tại trong danh mục!");
            inventory.Quantity += request.QuantityToAdd;
            // 5. Lưu xuống DB 1 lần duy nhất cho toàn bộ các thay đổi
bool isSaved = await _staffDal.SaveChangesAsync();
if (!isSaved)
{
    throw new Exception("Lỗi: Không thể lưu thay đổi vào cơ sở dữ liệu!");
}
var response = new StaffResponse
            {
                StaffId = staff.Id,
                ItemId = product.Id, 
                ItemName = product.Name, 
                Quantity = request.QuantityToAdd,
                NewStockQuantity = inventory.Quantity 
            };

            return response;
    }
}