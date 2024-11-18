using LagoVista.Core;
using LagoVista.Core.Attributes;
using LagoVista.Core.Commanding;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.Core.ViewModels;
using LagoVista.IoT.DeviceAdmin.Models;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceAccess
{
    public class EditDeviceViewModel : FormViewModelBase<Device>
    {
        public const string DeviceRepoId = "DEVICEREPOID";
        public const string DeviceId = "DEVICEID";

        string _deviceRepoId;
        string _deviceId;

        private void ShowProperties()
        {
            PerformNetworkOperation(async () =>
            {
                var result = await RestClient.GetAsync<List<CustomField>>($"/api/deviceconfig/{Model.DeviceConfiguration.Id}/properties");
                var fields = new Dictionary<string, FormField>();

                var adapter = new EditFormAdapter(Model.Properties, fields, ViewModelNavigation);
                if (result.Result != null)
                {
                    foreach (var field in result.Result)
                    {
                        var formField = FormField.Create(field.Key, new FormFieldAttribute(), null);

                        formField.Label = field.Label;

                        switch (field.FieldType.Value)
                        {
                            case ParameterTypes.State:
                                formField.Options = new List<EnumDescription>();
                                foreach (var state in field.StateSet.Value.States)
                                {
                                    formField.Options.Add(new EnumDescription() { Key = state.Key, Label = state.Name, Name = state.Name });
                                }

                                formField.FieldType = FormField.FieldType_Picker;

                                var initialState = field.StateSet.Value.States.Where(st => st.IsInitialState).FirstOrDefault();
                                if (initialState != null)
                                {
                                    formField.Value = initialState.Key;
                                }

                                break;
                            case ParameterTypes.String:
                                formField.FieldType = FormField.FieldType_Text;
                                formField.Value = field.DefaultValue;
                                break;
                            case ParameterTypes.DateTime:
                                formField.FieldType = FormField.FieldType_DateTime;
                                formField.Value = field.DefaultValue;
                                break;
                            case ParameterTypes.Integer:
                                formField.FieldType = FormField.FieldType_Integer;
                                formField.Value = field.DefaultValue;
                                break;
                            case ParameterTypes.Decimal:
                                formField.FieldType = FormField.FieldType_Decimal;
                                formField.Value = field.DefaultValue;
                                break;
                            case ParameterTypes.GeoLocation:
                                formField.FieldType = FormField.FieldType_Text;
                                formField.Value = field.DefaultValue;
                                break;
                            case ParameterTypes.TrueFalse:
                                formField.FieldType = FormField.FieldType_CheckBox;
                                formField.Value = field.DefaultValue;
                                break;
                            case ParameterTypes.ValueWithUnit:
                                formField.FieldType = FormField.FieldType_Decimal;
                                formField.Value = field.DefaultValue;
                                break;
                        }

                        formField.IsRequired = field.IsRequired;
                        formField.IsUserEditable = !field.IsReadOnly;

                        fields.Add(field.Key, formField);
                        adapter.AddViewCell(field.Key);
                    }
                }

                CustomFieldAdapter = adapter;
            });
        }

        EditFormAdapter _form;
        public EditFormAdapter CustomFieldAdapter
        {
            get { return _form; }
            set
            {
                _form = value;
                RaisePropertyChanged();
            }
        }

        protected override void BuildForm(EditFormAdapter form)
        {
            if (String.IsNullOrEmpty(Model.Name))
            {
                Model.Name = Model.DeviceId;
            }

            View[nameof(Model.DeviceId).ToFieldKey()].IsUserEditable = LaunchArgs.LaunchType == LaunchTypes.Create;

            form.AddViewCell(nameof(Model.Name));
            form.AddViewCell(nameof(Model.DeviceId));
            form.AddViewCell(nameof(Model.SerialNumber));
            form.AddViewCell(nameof(Model.DeviceType));
            form.AddViewCell(nameof(Model.GeoLocation));
            form.AddViewCell(nameof(Model.DisableWatchdog));

            ShowProperties();
        }


        public override Task<InvokeResult> SaveRecordAsync()
        {
            return PerformNetworkOperation(() =>
            {
                return FormRestClient.UpdateAsync($"/api/device/{_deviceRepoId}", this.Model);
            });
        }

        protected override string GetRequestUri()
        {
            _deviceRepoId = LaunchArgs.Parameters[EditDeviceViewModel.DeviceRepoId].ToString();
            _deviceId = LaunchArgs.Parameters[EditDeviceViewModel.DeviceId].ToString();

            return $"/api/device/{_deviceRepoId}/{_deviceId}/metadata";
        }

        public RelayCommand SetLocationCommand { get; }

    }
}
