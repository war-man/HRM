﻿using System;
using Ext.Net;
using Web.Core.Framework;
using Web.Core;
using System.Linq;
using SoftCore;
using Web.Core.Object.Security;
using Web.Core.Service.Security;

namespace Web.HJM.Modules.System
{
    public partial class SchedulerHistoryManagement : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ExtNet.IsAjaxRequest)
            {
                hdfDepartments.Text = string.Join(",", CurrentUser.Departments.Select(d => d.Id));

            }
            if (btnEditHistory.Visible)
            {
                gridScheduleHistory.Listeners.RowDblClick.Handler += " if(CheckSelectedRows(gridScheduleHistory)){btnUpdate.show();btnUpdateClose.hide()}";
            }

        }

        #region Event Method

        protected void InsertOrUpdate(object sender, DirectEventArgs e)
        {
            try
            {
                if (e.ExtraParams["Command"] == "Update")
                    Update();
                else
                    Insert();
                //reload data
                gridScheduleHistory.Reload();
            }
            catch (Exception ex)
            {
                ExtNet.MessageBox.Alert("Có lỗi xảy ra", ex.Message).Show();
            }
        }

        private void Insert()
        {
            try
            {
                var util = new Util();
                var schedulerHistory = new SchedulerHistory();
                int schedulerId;
                if (int.TryParse(cbxScheduler.SelectedItem.Value, out schedulerId))
                    schedulerHistory.SchedulerId = schedulerId;
                if (!string.IsNullOrEmpty(txtDescription.Text)) schedulerHistory.Description = txtDescription.Text.Trim();
                schedulerHistory.HasError = chkHasError.Checked;
                if (!string.IsNullOrEmpty(txtErrorMessage.Text))
                    schedulerHistory.ErrorMessage = txtErrorMessage.Text.Trim();
                if (!string.IsNullOrEmpty(txtErrorDescription.Text))
                    schedulerHistory.ErrorDescription = txtErrorDescription.Text.Trim();
                if (!util.IsDateNull(txtStartTime.SelectedDate))
                {
                    schedulerHistory.StartTime = txtStartTime.SelectedDate;
                }
                if (!util.IsDateNull(txtEndTime.SelectedDate))
                {
                    schedulerHistory.EndTime = txtEndTime.SelectedDate;
                }
                SchedulerHistoryServices.Create(schedulerHistory);

            }
            catch (Exception ex)
            {
                Dialog.Alert("Có lỗi xảy ra trong quá trình thêm mới: {0}".FormatWith(ex.Message));
            }
        }

        private void Update()
        {
            try
            {
                var util = new Util();
                if (string.IsNullOrEmpty(hdfKeyRecord.Text)) return;
                var schedulerHistory = SchedulerHistoryServices.GetById(Convert.ToInt32(hdfKeyRecord.Text));
                if (schedulerHistory == null) return;
                int schedulerId;
                if (int.TryParse(cbxScheduler.SelectedItem.Value, out schedulerId))
                    schedulerHistory.SchedulerId = schedulerId;
                if (!string.IsNullOrEmpty(txtDescription.Text)) schedulerHistory.Description = txtDescription.Text.Trim();
                schedulerHistory.HasError = chkHasError.Checked;
                if (!string.IsNullOrEmpty(txtErrorMessage.Text))
                    schedulerHistory.ErrorMessage = txtErrorMessage.Text.Trim();
                if (!string.IsNullOrEmpty(txtErrorDescription.Text))
                    schedulerHistory.ErrorDescription = txtErrorDescription.Text.Trim();
                if (!util.IsDateNull(txtStartTime.SelectedDate))
                {
                    schedulerHistory.StartTime = txtStartTime.SelectedDate;
                }
                if (!util.IsDateNull(txtEndTime.SelectedDate))
                {
                    schedulerHistory.EndTime = txtEndTime.SelectedDate;
                }
                SchedulerHistoryServices.Update(schedulerHistory);
            }
            catch (Exception ex)
            {
                Dialog.Alert("Có lỗi xảy ra trong quá trình cập nhật: {0}".FormatWith(ex.Message));
            }
        }

        [DirectMethod]
        public void ResetForm()
        {
            cbxScheduler.Reset();
            txtDescription.Reset();
            txtErrorMessage.Reset();
            txtErrorDescription.Reset();
            txtStartTime.Reset();
            txtEndTime.Reset();
            chkHasError.Reset();
        }

        protected void Delete(object sender, DirectEventArgs directEventArgs)
        {
            try
            {
                int id;
                if (!int.TryParse(hdfKeyRecord.Text, out id) || id <= 0) return;
                SchedulerHistoryServices.Delete(id);
                gridScheduleHistory.Reload();
                RM.RegisterClientScriptBlock("Grid_Reload", "ReloadGrid();");
            }
            catch (Exception ex)
            {
                Dialog.Alert("Có lỗi xảy ra trong quá trình xóa: {0}".FormatWith(ex.Message));
            }
        }
        [DirectMethod]
        protected void EditSchedulerHistory_Click(object sender, DirectEventArgs e)
        {
            int id;
            if (!int.TryParse(hdfKeyRecord.Text, out id) || id <= 0) return;
            var schedulerHistory = SchedulerHistoryServices.GetById(id);
            if (schedulerHistory != null)
            {
                cbxScheduler.SelectedItem.Text = schedulerHistory.SchedulerName;
                txtDescription.Text = schedulerHistory.Description;
                chkHasError.Checked = schedulerHistory.HasError;
                txtErrorMessage.Text = schedulerHistory.ErrorMessage;
                txtErrorDescription.Text = schedulerHistory.ErrorDescription;
                txtStartTime.SelectedDate = schedulerHistory.StartTime;
                txtEndTime.SelectedDate = schedulerHistory.EndTime;
            }
            // show window
            btnUpdate.Show();
            btnUpdateClose.Hide();

            wdTimeSheetRule.Title = @"Cập nhật quản lý lịch sử tiến trình";
            wdTimeSheetRule.Show();
        }

        #endregion


        protected void storeScheduler_OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            storeScheduler.DataSource = SchedulerServices.GetAll();
            storeScheduler.DataBind();
        }

        protected void SeeDetail(object sender, DirectEventArgs e)
        {

        }

    }

}
