using System.Text.Json.Serialization;
using be_lspmpi.Models;
using be_lspmpi.Dto;

namespace be_lspmpi.Json
{
    [JsonSerializable(typeof(User[]))]
    [JsonSerializable(typeof(User))]
    [JsonSerializable(typeof(Role))]
    [JsonSerializable(typeof(UserProfile))]
    [JsonSerializable(typeof(Article))]
    [JsonSerializable(typeof(Article[]))]
    [JsonSerializable(typeof(ArticleCategory))]
    [JsonSerializable(typeof(ArticleCategory[]))]
    [JsonSerializable(typeof(ServiceResponse))]
    [JsonSerializable(typeof(PaginatedResponse<User>))]
    [JsonSerializable(typeof(PaginatedResponse<Article>))]
    [JsonSerializable(typeof(PaginatedResponse<ArticleCategory>))]
    [JsonSerializable(typeof(LoginRequest))]
    [JsonSerializable(typeof(LoginResponse))]
    [JsonSerializable(typeof(RegisterRequest))]
    [JsonSerializable(typeof(CreateUserRequest))]
    [JsonSerializable(typeof(ChangePasswordRequest))]
    [JsonSerializable(typeof(ChangeForgotPasswordRequest))]
    [JsonSerializable(typeof(PhotoUploadRequest))]
    [JsonSerializable(typeof(PhotoUploadResponse))]
    [JsonSerializable(typeof(CreateArticleDto))]
    [JsonSerializable(typeof(UpdateArticleDto))]
    [JsonSerializable(typeof(CreateArticleCategoryDto))]
    [JsonSerializable(typeof(UpdateArticleCategoryDto))]
    [JsonSerializable(typeof(FindRequest))]
    internal partial class AppJsonSerializerContext : JsonSerializerContext
    {
    }
}