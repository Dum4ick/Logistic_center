using LogisticCenter.ViewModels;
using System.Net.Http.Json;
using System.Text.Json;

namespace LogisticCenter.Data;

public class ApiData
{
    const string LocalBaseUrl = "http://localhost/logistic_center/";
    const string RemoteBaseUrl = "http://f1196925.xsph.ru/logistic_center/";

    const string CurrentURL = LocalBaseUrl;

    //HttpClient для отправки запросов к серверу
    private readonly HttpClient _httpClient;

    public ApiData()
    {
        //Создаём HttpClient
        _httpClient = new HttpClient();
    }

    // Получение списка всех пользователей с сервера
    public async Task<List<UserModel>> GetUsers()
    {
        //Подключение скрипта для получения спискка пользвоателей
        const string url = $"{CurrentURL}get_all_users.php";

        try
        {
            //Отправляем GET запрос на сервер
            var response = await _httpClient.GetAsync(url);

            //Получаем ответ и преобразуем JSON в объект ApiResponse где User это список пользователей
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<UserModel>>>();

            //Если result или result.User раняется null то возвращаем пустой список, иначе возваращаем result.User
            return result?.Data ?? new List<UserModel>();

        }
        catch
        {
            //Если чтото не так то просто возвращаем пустой список
            return new List<UserModel>();
        }
    }

    //Авторизация пользователя
    public async Task<(bool success, UserModel user, string message)> LoginAsync(string email, string password)
    {
        //Подключение скрипта для авторизации пользвоателей
        const string url = $"{CurrentURL}login.php";

        try
        {
            //Отправляем POST запрос с email и паролем
            var response = await _httpClient.PostAsJsonAsync(url, new
            {
                email,
                password
            });

            //Читаем ответ сервера и преобразуем JSON в ApiResponse<UserModel>
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserModel>>();

            // Если статус success и result не null то возвращаем что вход удался
            if (result?.Status == "success") return (true, result.User, null);

            //Если сервер вернул ошибку то выводим ошибку
            return (false, null, result?.Message ?? "Ошибка входа");
        }
        catch (Exception ex)
        {
            return (false, null, ex.Message);
        }
    }

    //Регистрация нового пользователя
    public async Task<string> RegisterUser(UserModel user)
    {
        //Подключение скрипта для регистрации пользвоателей
        const string url = $"{CurrentURL}reg.php";

        try
        {
            //Отправляем POST запрос с данными пользователя
            var response = await _httpClient.PostAsJsonAsync(url, new
            {
                username = user.Name,
                email = user.Email,
                password = user.Password
            });

            //Читаем ответ сервера
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

            //Если сервер вернул статус ok то регистрация прошла успешно
            if (result != null && result.Status == "ok") return "Регистрация прошла успешно";

            //Возвращаем сообщение ошибки от сервера
            return result?.Message ?? "Ошибка регистрации";
        }
        catch (Exception ex)
        {
            return $"Ошибка сети: {ex.Message}";
        }
    }

    // Обновление имени пользователя (full_name)
    public async Task<string> UpdateFullNameAsync(int userId, string fullName)
    {
        const string url = $"{CurrentURL}update_fullname.php";

        try
        {
            var response = await _httpClient.PostAsJsonAsync(url, new
            {
                id = userId,
                full_name = fullName
            });

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

            if (result != null && result.Status == "success")
                return "OK";

            return result?.Message ?? "Ошибка обновления имени";
        }
        catch (Exception ex)
        {
            return $"Ошибка сети: {ex.Message}";
        }
    }

    public async Task<List<ProductModel>> GetProducts(string search = "")
    {
        string url = $"{CurrentURL}get_products.php?search={search}";
        var result = await _httpClient
            .GetFromJsonAsync<ApiResponse<List<ProductModel>>>(url);
        return result?.Data ?? new();
    }

    public async Task<(bool success, string message)> AddProduct(ProductModel p)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}add_product.php", p);

        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse<object>>();

        if (result?.Status == "success")
            return (true, result.Message);

        return (false, result?.Message ?? "Ошибка добавления товара");
    }


    public async Task<(bool success, string message)> UpdateProduct(ProductModel p)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}update_product.php", p);

        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse<object>>();

        if (result?.Status == "success")
            return (true, result.Message);

        return (false, result?.Message ?? "Ошибка обновления товара");
    }


    public async Task<bool> DeleteProduct(int id)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}delete_product.php",
            new { id });

        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse<object>>();

        return result?.Status == "success";
    }


    public async Task<(bool success, string message)> DeleteUserAsync(string userId)
    {
        const string url = $"{CurrentURL}delete_user.php";

        try
        {
            var response = await _httpClient.PostAsJsonAsync(url, new
            {
                id = userId
            });

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<object>>();

            if (result?.Status == "success")
                return (true, "Пользователь удалён");

            return (false, result?.Message ?? "Ошибка удаления");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<List<RoleModel>> GetRolesAsync()
    {
        const string url = $"{CurrentURL}get_roles.php";

        var result = await _httpClient
            .GetFromJsonAsync<ApiResponse<List<RoleModel>>>(url);

        return result?.Data ?? new();
    }


    public async Task<(bool success, string message)> AssignRoleAsync(int userId, int roleId)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}assign_role.php",
            new { user_id = userId, role_id = roleId });

        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse<object>>();

        if (result?.Status == "success")
            return (true, result.Message);

        return (false, result?.Message ?? "Ошибка назначения роли");
    }

    public async Task<(bool success, string message)> UpdateUserAsync(
    int userId,
    string fullName,
    string email)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}update_user.php",
            new
            {
                id = userId,
                full_name = fullName,
                email = email
            });

        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse<object>>();

        if (result?.Status == "success")
            return (true, "Данные обновлены");

        return (false, result?.Message ?? "Ошибка обновления");
    }


    public async Task<List<CategoryModel>> GetCategories()
    {
        var response = await _httpClient
            .GetFromJsonAsync<ApiResponse<List<CategoryModel>>>(
                $"{CurrentURL}get_categories.php");

        return response?.Data ?? new();
    }

    public async Task<List<StockModel>> GetStockAsync()
    {
        const string url = $"{CurrentURL}get_stock.php";

        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<StockModel>>>(url);
            return response?.Data ?? new List<StockModel>();
        }
        catch (Exception ex)
        {
            // Можно логировать ex.Message
            return new List<StockModel>();
        }
    }


    public async Task<List<StockModel>> GetStockWarehouseAsync(int warehouseId)
    {
        var url = $"{CurrentURL}get_stock.php?warehouse_id={warehouseId}";

        var response =
            await _httpClient.GetFromJsonAsync<ApiResponse<List<StockModel>>>(url);

        return response?.Data ?? new List<StockModel>();
    }




    public async Task<(bool success, string message)> AddStockAsync(
        int productId,
        int warehouseId,
        int quantity)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}product_receipt.php",
            new
            {
                product_id = productId,
                warehouse_id = warehouseId,
                quantity
            });

        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse<object>>();

        if (result?.Status == "success")
            return (true, result.Message);

        return (false, result?.Message ?? "Ошибка поступления");
    }

    public async Task<List<WarehouseModel>> GetWarehousesAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<ApiResponse<List<WarehouseModel>>>(
            $"{CurrentURL}get_warehouses.php");

        return result?.Data ?? new();
    }

    public async Task<(bool success, string message)> WriteOffStockAsync(
    int productId,
    int warehouseId,
    int quantity)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}write-off_product.php",
            new
            {
                product_id = productId,
                warehouse_id = warehouseId,
                quantity
            });

        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse<object>>();

        if (result?.Status == "success")
            return (true, result.Message);

        return (false, result?.Message ?? "Ошибка списания");
    }

    public async Task<List<OrderModel>> GetOrdersAsync()
    {
        const string url = $"{CurrentURL}get_orders.php";

        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<OrderModel>>>(url);
            return response?.Data ?? new List<OrderModel>();
        }
        catch (Exception ex)
        {
            return new List<OrderModel>();
        }
    }


    public async Task<(bool success, int orderId, string message)> CreateOrderAsync(
    int userId, int warehouseId)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}create_order.php",
            new { user_id = userId, warehouse_id = warehouseId });

        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse<int>>();

        if (result?.Status == "success")
            return (true, result.Data, null);

        return (false, 0, result?.Message ?? "Ошибка создания заказа");
    }



    public async Task<(bool success, string message)> AddOrderItemAsync(
    int orderId,
    int productId,
    int quantity,
    decimal price)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}add_order_item.php",
            new
            {
                order_id = orderId,
                product_id = productId,
                quantity,
                price
            });

        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse<object>>();

        if (result?.Status == "success")
            return (true, null);

        return (false, result?.Message ?? "Ошибка добавления товара");
    }


    public async Task<List<OrderItemModel>> GetOrderItemsAsync(int order_id)
    {
        var url = $"{CurrentURL}get_order_items.php?order_id={order_id}";

        var response =
            await _httpClient.GetFromJsonAsync<ApiResponse<List<OrderItemModel>>>(url);

        return response?.Data ?? new List<OrderItemModel>();
    }

    public async Task<List<OrderStatusModel>> GetOrderStatusesAsync()
    {
        const string url = $"{CurrentURL}get_order_statuses.php";

        try
        {
            var response =
                await _httpClient.GetFromJsonAsync<ApiResponse<List<OrderStatusModel>>>(url);

            return response?.Data ?? new();
        }
        catch
        {
            return new();
        }
    }


    public async Task<OrderStatusModel> GetOrderStatusAsync(int orderId)
    {
        var url = $"{CurrentURL}get_order_status.php?order_id={orderId}";

        try
        {
            var response =
                await _httpClient.GetFromJsonAsync<ApiResponse<OrderStatusModel>>(url);

            return response?.Data;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, int statusId)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}update_order_status.php",
            new
            {
                order_id = orderId,
                status_id = statusId
            });

        var result =
            await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

        return result?.Status == "success";
    }

    public async Task<(bool success, int shipmentId, string message)> ConfirmOrderAsync(int orderId)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}confirm_order.php",
            new { order_id = orderId });

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<int>>();

        if (result?.Status == "success")
            return (true, result.Data, null);

        return (false, 0, result?.Message ?? "Ошибка подтверждения заказа");
    }


    public async Task ShipOrderAsync(int orderId)
    {
        await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}ship_order.php",
            new { order_id = orderId });
    }

    public async Task CompleteOrderAsync(int orderId)
    {
        await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}complete_delivery.php",
            new { order_id = orderId });
    }



    public async Task<ApiResult> CancelOrderAsync(int orderId)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}cancel_order.php",
            new { order_id = orderId });

        return await response.Content.ReadFromJsonAsync<ApiResult>();
    }

    public async Task<ApiResult> DeleteOrderAsync(int orderId)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}delete_order.php",
            new { order_id = orderId });

        return await response.Content.ReadFromJsonAsync<ApiResult>();
    }

    // ===== SHIPMENTS =====

    public async Task<List<ShipmentModel>> GetShipmentsAsync()
    {
        var response = await _httpClient
            .GetFromJsonAsync<ApiResponse<List<ShipmentModel>>>(
                $"{CurrentURL}get_shipments.php");

        return response?.Data ?? new();
    }

    public async Task<List<ShipmentItemModel>> GetShipmentItemsAsync(int shipmentId)
    {
        var url = $"{CurrentURL}get_shipment_items.php?shipment_id={shipmentId}";

        var response =
            await _httpClient.GetFromJsonAsync<ApiResponse<List<ShipmentItemModel>>>(url);

        return response?.Data ?? new();
    }


    public async Task<List<ShipmentStatusModel>> GetShipmentStatusesAsync()
    {
        var response = await _httpClient
            .GetFromJsonAsync<ApiResponse<List<ShipmentStatusModel>>>(
                $"{CurrentURL}get_shipment_statuses.php");

        return response?.Data ?? new();
    }

    public async Task<bool> UpdateShipmentStatusAsync(int shipmentId, int statusId)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}update_shipment_status.php",
            new { shipment_id = shipmentId, status_id = statusId });

        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse<object>>();

        return result?.Status == "success";
    }

    public async Task<ShipmentStatusModel> GetShipmentStatusAsync(int shipmentId)
    {
        var url = $"{CurrentURL}get_shipment_status.php?shipment_id={shipmentId}";

        try
        {
            var response =
                await _httpClient.GetFromJsonAsync<ApiResponse<ShipmentStatusModel>>(url);

            Console.WriteLine(response?.Data);

            return response?.Data;
        }
        catch
        {
            return null;
        }
    }

    public async Task<(bool success, string message)> AddWarehouseAsync(WarehouseModel w)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}add_warehouse.php", w);

        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse<object>>();

        if (result?.Status == "success")
            return (true, result.Message);

        return (false, result?.Message ?? "Ошибка добавления склада");
    }

    public async Task<(bool success, string message)> UpdateWarehouseAsync(WarehouseModel w)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}update_warehouse.php", w);

        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse<object>>();

        if (result?.Status == "success")
            return (true, result.Message);

        return (false, result?.Message ?? "Ошибка обновления склада");
    }

    public async Task<(bool success, string message)> DeleteWarehouseAsync(int id)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{CurrentURL}delete_warehouse.php",
            new { id });

        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse<object>>();

        if (result?.Status == "success")
            return (true, "Склад удалён");

        return (false, result?.Message ?? "Ошибка удаления склада");
    }
    public async Task<List<StockReportModel>> GetStockReportAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<ApiResponse<List<StockReportModel>>>(
            $"{CurrentURL}get_stock_report.php");

        return result?.Data ?? new();
    }

    public async Task<List<ShipmentReportModel>> GetShipmentReportAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<ApiResponse<List<ShipmentReportModel>>>(
            $"{CurrentURL}get_shipment_report.php");

        return result?.Data ?? new();
    }

    public async Task<FinanceReportModel> GetFinanceReportAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<ApiResponse<FinanceReportModel>>(
            $"{CurrentURL}get_finance_report.php");

        return result?.Data ?? new();
    }



}
