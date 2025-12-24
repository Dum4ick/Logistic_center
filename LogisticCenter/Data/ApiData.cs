using LogisticCenter.ViewModels;
using System.Net.Http.Json;
using System.Text.Json;

namespace LogisticCenter.Data;

public class ApiData
{
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
        const string url = "http://f1196925.xsph.ru/get_all_users.php";

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
        const string url = "http://f1196925.xsph.ru/login.php";

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
        const string url = "http://f1196925.xsph.ru/reg.php";

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
        const string url = "http://f1196925.xsph.ru/update_fullname.php";

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
        string url = $"http://f1196925.xsph.ru/get_products.php?search={search}";
        var result = await _httpClient
            .GetFromJsonAsync<ApiResponse<List<ProductModel>>>(url);
        return result?.Data ?? new();
    }

    public async Task<bool> AddProduct(ProductModel p)
    {
        var r = await _httpClient.PostAsJsonAsync(
            "http://f1196925.xsph.ru/add_product.php", p);
        return r.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateProduct(ProductModel p)
    {
        var r = await _httpClient.PostAsJsonAsync(
            "http://f1196925.xsph.ru/update_product.php", p);
        return r.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var r = await _httpClient.PostAsJsonAsync(
            "http://f1196925.xsph.ru/delete_product.php",
            new { id });
        return r.IsSuccessStatusCode;
    }

    public async Task<(bool success, string message)> DeleteUserAsync(string userId)
    {
        const string url = "http://f1196925.xsph.ru/delete_user.php";

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
        const string url = "http://f1196925.xsph.ru/get_roles.php";

        var result = await _httpClient
            .GetFromJsonAsync<ApiResponse<List<RoleModel>>>(url);

        return result?.Data ?? new();
    }


    public async Task<(bool success, string message)> AssignRoleAsync(int userId, int roleId)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "http://f1196925.xsph.ru/assign_role.php",
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
            "http://f1196925.xsph.ru/update_user.php",
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

    public async Task<List<UserModel>> SearchUsersAsync(string search, int roleId)
    {
        string url =
            $"http://f1196925.xsph.ru/search_users.php?search={Uri.EscapeDataString(search ?? "")}&role_id={roleId}";

        var result = await _httpClient
            .GetFromJsonAsync<ApiResponse<List<UserModel>>>(url);

        return result?.Data ?? new();
    }




}
