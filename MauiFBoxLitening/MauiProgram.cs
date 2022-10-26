using Microsoft.AspNetCore.Components.WebView.Maui;
using MauiFBoxLitening.Data;

namespace MauiFBoxLitening;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddBootstrapBlazor();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddSingleton<WeatherForecastService>();

        Random r = new Random(Guid.NewGuid().GetHashCode());
        for (int i = 0; i < 3; i++)
        {
            Caches.boxgroups.Add(new box_groups()
            {
                uid = r.Next(10000,99999),
                name = "盒子组_" + i
            });
        }
        foreach (var boxgroup in Caches.boxgroups)
        {
            for (int i = 0; i < 15; i++)
            {
                Caches.boxs.Add(new box()
                {
                    boxNo = GenerateUniqueText(8),
                    alias = boxgroup.name + "_盒子_" + i.ToString(),
                    connectionState = "在线",
                    boxType = "NULL",
                    networkType = "NULL",
                    uid = boxgroup.uid,
                    name = boxgroup.name
                });
            }
        }
        foreach (var box in Caches.boxs)
        {
            for (int i = 0; i < 2; i++)
            {
                Caches.dmonsgroups.Add(new dmon_group()
                {
                    GroupId = r.Next(10000, 99999),
                    GroupName = "监控分组_" + i.ToString(),
                    BoxNo = box.boxNo
                });
            }
        }
        foreach (var dmongroup in Caches.dmonsgroups)
        {
            for (int i = 0; i < 5; i++)
            {
                Caches.dmons.Add(new dmon()
                {
                    Dmondtov2Name = "监控点_" + i.ToString(),
                    FractionalDigits = "2",
                    GroupId = dmongroup.GroupId,
                    GroupName = dmongroup.GroupName,
                    Unit = "NuLL",
                    Privilege = "NULL",
                    TrafficSaving = 1,
                    DeadValue = 2,
                    Memo = "NULL",
                    Encoding = "UTF-8",
                    StringByteOrder = "NULL",
                    CharCount = 8,
                    Id = r.Next(10000, 99999),
                    IsDeviceChanged = 0,
                    TaskState = "NULL"
                });
            }
        }
        return builder.Build();
    }
    /// <summary>
    /// 生成特定位数的唯一字符串
    /// </summary>
    /// <param name="num">特定位数</param>
    /// <returns></returns>
    public static string GenerateUniqueText(int num)
    {
        string randomResult = string.Empty;
        string readyStr = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        char[] rtn = new char[num];
        Guid gid = Guid.NewGuid();
        var ba = gid.ToByteArray();
        for (var i = 0; i < num; i++)
        {
            rtn[i] = readyStr[((ba[i] + ba[num + i]) % 61)];
        }
        foreach (char r in rtn)
        {
            randomResult += r;
        }
        return randomResult;
    }
}
