using BootstrapBlazor.Components;
using MauiFBoxLitening.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Button = BootstrapBlazor.Components.Button;
using Timer = System.Threading.Timer;

namespace MauiFBoxLitening.Pages
{
    public class recordModel
    {
        public string value { get; set; }
        public string time { get; set; }
    }

    partial class Cards : ComponentBase
    {
        [Parameter]
        public string boxid { get; set; }

        public bool IsOpen { get; set; } = false;

        public long dmon_id { get; set; }
        private string _cachevalue = "NULL";
        public string cachevalue
        {
            get => _cachevalue;
            set
            {
                _cachevalue = value;
                cValue = value;
                for (int i = 0; i < 5; i++)
                {
                    linearr[i] = linearr[i + 1];
                }
                try { linearr[5] = Convert.ToDouble(value); } catch (Exception) { linearr[5] = 0; }
            }
        }
        public string cValue { get; set; } = "NULL";
        public List<recordModel> record = new List<recordModel>();
        public Timer timer { get; set; }
        double[] linearr = new double[6] { 0, 0, 0, 0, 0, 0 };
        string[] time = new string[6] {
            "-5",
            "-4",
            "-3",
            "-2",
            "-1",
            "现在" };
        private Chart? LineChart { get; set; }
        public async Task OnClick(long? id)
        {
            dmon_id = (long)id;
            record = new List<recordModel>();
            linearr = new double[6] { 0, 0, 0, 0, 0, 0 };
            int num = 0;
            timer = new Timer(async (object? stateInfo) =>
            {
                cachevalue = new Random(Guid.NewGuid().GetHashCode()).Next(5000, 5999).ToString();
                await Update(LineChart);
            }, new AutoResetEvent(false), 1000, 1000);
            IsOpen = true;
        }
        public static Task Update(Chart chart) => chart.Update(ChartAction.Update);
        private Task<ChartDataSource> OnInit(float tension, bool hasNull)
        {
            var ds = new ChartDataSource();
            ds.Options.Title = "监控点"; 
            ds.Options.X.Title = "时间";
            ds.Options.Y.Title = "value";
            ds.Labels = time;
            List<object> ints = new List<object>();
            foreach (var item in linearr)
            {
                ints.Add(Convert.ToInt32(item));
            }
            ds.Data.Add(new ChartDataset()
            {
                Tension = tension,
                Label = "value",
                Data = ints
            });
            return Task.FromResult(ds);
        }
        private Task OnAfterInit()
        {
            return Task.CompletedTask;
        }
        private Task OnAfterUpdate(ChartAction action)
        {
            InvokeAsync(StateHasChanged);
            return Task.CompletedTask;
        }
    }
}
