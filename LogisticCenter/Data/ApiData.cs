using LogisticCenter;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Security;
using System;
using System.Net.Http.Json;
using System.Text.Json;

public class ApiData
{
	private readonly HttpClient _httpClient;

	public ApiData()
	{
		_httpClient = new HttpClient();
	}

	public async Task<List<UserModel>> GetUsers()
	{
		string url = "http://f1196925.xsph.ru/get_users.php";

		try
		{
			var response = await _httpClient.GetStringAsync(url);
            var result = JsonSerializer.Deserialize<DataModel>(response);
            return result?.UsersList ?? new List<UserModel>();
        }

		catch
		{
			return new List<UserModel>();
		}

	}

    public async Task<string> RegisterUser(UserModel user)
    {
        string url = "http://f1196925.xsph.ru/reg.php";

        try
        {
            var response = await _httpClient.PostAsJsonAsync(url, new
            {
                username = user.Name,
                email = user.Email,
                password = user.Password
            });

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(content);

            if (result != null && result.ContainsKey("status"))
            {
                return result.ContainsKey("message") ? result["message"] : "Ответ сервера без сообщения";
            }

            return "Не удалось зарегистрировать пользователя: некорректный ответ сервера";
        }
        catch (Exception ex)
        {
            return $"Ошибка при регистрации: {ex.Message}";
        }
    }

}
