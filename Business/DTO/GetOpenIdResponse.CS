
/// <summary>
/// 获取用户授权第二步返回实体
/// </summary>
public class GetOpenIdResponse
{
    public string access_token { get; set; }
    public string expires_in { get; set; }
    public string refresh_token { get; set; }
    public string openid { get; set; }
    public string scope { get; set; }
    public string errcode { get; set; }
    public string errmsg { get; set; }
}