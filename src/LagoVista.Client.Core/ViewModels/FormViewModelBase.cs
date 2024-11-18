using LagoVista.Client.Core.Net;
using LagoVista.Core.Models.UIMetaData;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using LagoVista.Core.Models;
using LagoVista.Core.ViewModels;
using System.Threading.Tasks;
using LagoVista.Core;
using LagoVista.Core.Validation;
using LagoVista.Client.Core.Resources;
using LagoVista.Client.Core.ViewModels.Orgs;
using LagoVista.Core.Attributes;

namespace LagoVista.Client.Core.ViewModels
{
    public abstract class FormViewModelBase<TModel> : AppViewModelBase, IFormParentViewModel where TModel : new()
    {
        IFormRestClient<TModel> _restClient;

        public FormViewModelBase()
        {
            _restClient = new FormRestClient<TModel>(base.RestClient);
        }


        public IFormRestClient<TModel> FormRestClient { get { return _restClient; } }


        TModel _model;
        public TModel Model
        {
            get { return _model; }
            set { Set(ref _model, value); }
        }

        IDictionary<string, FormField> _view;
        public IDictionary<string, FormField> View
        {
            get { return _view; }
            set { Set(ref _view, value); }
        }

        public bool ViewToModel(EditFormAdapter form, TModel model)
        {
            var modelProperties = typeof(TModel).GetTypeInfo().DeclaredProperties;

            if (!form.Validate())
            {
                return false;
            }

            foreach (var formItem in FormAdapter.FormItems)
            {
                var prop = modelProperties.Where(prp => prp.Name.ToLower() == formItem.Name.ToLower()).FirstOrDefault();
                switch (formItem.FieldType)
                {
                    case FormField.FeildType_EntityHeaderPicker:
                        break;
                    case FormField.FieldType_CheckBox:
                        if (bool.TryParse(formItem.Value, out bool result))
                        {
                            prop.SetValue(model, result);
                        }
                        break;
                    case FormField.FieldType_Picker:
                        if (String.IsNullOrEmpty(formItem.Value))
                        {
                            prop.SetValue(model, null);
                        }
                        else
                        {
                            if (prop.PropertyType == typeof(String))
                            {
                                prop.SetValue(model, formItem.Options.Where(opt => opt.Key == formItem.Value).First().Key);
                            }
                            else
                            {
                                var eh = Activator.CreateInstance(prop.PropertyType) as EntityHeader;
                                eh.Id = formItem.Value;
                                eh.Text = formItem.Options.Where(opt => opt.Key == formItem.Value).First().Label;
                                prop.SetValue(model, eh);
                            }
                        }
                        break;
                    case FormField.FieldType_Integer:
                        if (!String.IsNullOrEmpty(formItem.Value))
                        {
                            if (int.TryParse(formItem.Value, out int intValue))
                            {
                                prop.SetValue(model, intValue);
                            }
                        }

                        break;
                    case FormField.FieldType_Decimal:
                        if (!String.IsNullOrEmpty(formItem.Value))
                        {
                            if (double.TryParse(formItem.Value, out double intValue))
                            {
                                prop.SetValue(model, intValue);
                            }
                        }

                        break;
                    case FormField.FieldType_NameSpace:
                    case FormField.FieldType_MultilineText:
                    case FormField.FieldType_Text:
                    case FormField.FieldType_Key:
                        prop.SetValue(model, formItem.Value);
                        break;
                    case FormField.FieldType_Password:
                        if (!String.IsNullOrEmpty(formItem.Value))
                        {
                            prop.SetValue(model, formItem.Value);
                        }
                        break;
                }
            }

            return true;
        }

        public override Task<bool> CanCancelAsync()
        {
            if ((FormAdapter != null && FormAdapter.IsDirty) || HasDirtyChildren)
            {
                return Popups.ConfirmAsync(ClientResources.Confirm_Unsaved_Title, ClientResources.Confirm_Unsaved_Message);
            }
            else
            {
                return Task.FromResult(true);
            }

        }

        public void ModelToView(TModel model, EditFormAdapter form)
        {
            var modelProperties = typeof(TModel).GetTypeInfo().DeclaredProperties;

            foreach (var formItem in form.FormItems)
            {
                if (formItem.FieldType != FieldTypes.LinkButton.ToString())
                {
                    var prop = modelProperties.Where(prp => prp.Name.ToLower() == formItem.Name.ToLower()).FirstOrDefault();
                    var value = prop.GetValue(model);

                    switch (formItem.FieldType)
                    {
                        case FormField.FieldType_Picker:
                            if (value != null)
                            {
                                if (value.GetType() == typeof(String))
                                {
                                    formItem.Value = value.ToString();
                                }
                                else
                                {
                                    var entityHeader = value as EntityHeader;
                                    formItem.Value = entityHeader.Id;
                                }
                            }
                            break;
                        case FormField.FeildType_EntityHeaderPicker:
                            var ehValue = value as EntityHeader;
                            if (ehValue != null)
                            {
                                formItem.Value = ehValue.Id;
                                formItem.Display = ehValue.Text;
                            }
                            break;
                        case FormField.FieldType_Bool:
                        case FormField.FieldType_NameSpace:
                        case FormField.FieldType_CheckBox:
                        case FormField.FieldType_Integer:
                        case FormField.FieldType_Decimal:
                        case FormField.FieldType_MultilineText:
                        case FormField.FieldType_Text:
                        case FormField.FieldType_Key:
                            if (value != null)
                            {
                                formItem.Value = value.ToString();
                            }
                            break;
                    }
                }
            }
        }

        public bool IsEdit
        {
            get { return LaunchArgs != null && LaunchArgs.LaunchType == LaunchTypes.Edit; }
        }

        public bool IsCreate
        {
            get { return LaunchArgs != null && LaunchArgs.LaunchType == LaunchTypes.Create; }
        }


        EditFormAdapter _form;
        public EditFormAdapter FormAdapter
        {
            get { return _form; }
            set
            {
                _form = value;
                RaisePropertyChanged();
            }
        }

        public override Task ReloadedAsync()
        {
            if (FormAdapter != null)
            {
                FormAdapter.Refresh();
            }

            return base.ReloadedAsync();
        }

        protected async Task<InvokeResult> LoadView()
        {
            InvokeResult<DetailResponse<TModel>> result;

            if (LaunchArgs.LaunchType == LaunchTypes.Edit)
            {
                result = await FormRestClient.GetAsync(GetRequestUri());
            }
            else if (LaunchArgs.LaunchType == LaunchTypes.Create || this.GetType() == typeof(OrgEditorViewModel))
            {
                //HACK: On OrgEditViewModel, but is all contained in this assembly, no apps should care about it, we eventually need to launch an initial view in CreateMode, but need to make updates to the core, otherwise OrgEditorViewModel is launched as LaunchType = Other but it's really created
                result = await FormRestClient.CreateNewAsync(GetRequestUri());
            }
            else
            {
                throw new Exception("ViewModels based on FormViewModelBase only support Edit and Create launch types.");
            }

            if (result.Successful)
            {
                var detailView = result.Result;
                if (LaunchArgs.LaunchType == LaunchTypes.Edit && LaunchArgs.Child != null)
                {
                    Model = (TModel)LaunchArgs.Child;
                }
                else
                {
                    Model = detailView.Model;
                }

                View = detailView.View;
                if (View.ContainsKey("key"))
                {
                    View["key"].IsUserEditable = LaunchArgs.LaunchType == LaunchTypes.Create;
                }
                var form = new EditFormAdapter(Model, detailView.View, ViewModelNavigation);
                form.OptionSelected += Form_OptionSelected;
                form.EHPickerTapped += Form_EHPickerTapped;
                BuildForm(form);
                ModelToView(Model, form);
                FormAdapter = form;
            }

            return result.ToInvokeResult();
        }

        private void Form_EHPickerTapped(object sender, string e)
        {
            EHPickerTapped(e);
        }

        public virtual void EHPickerTapped(string fieldName)
        {

        }

        public abstract Task<InvokeResult> SaveRecordAsync();

        public override async void Save()
        {
            if (!FormAdapter.IsDirty && !HasDirtyChildren && LaunchArgs.LaunchType == LaunchTypes.Edit)
            {
                await ViewModelNavigation.GoBackAsync();
            }
            else if (FormAdapter.Validate())
            {
                ViewToModel(FormAdapter, Model);
                if (Model is IValidateable)
                {
                    var result = Validator.Validate(Model as IValidateable);
                    if (!result.Successful)
                    {

                        await ShowValidationErrorsAsync(result);
                        return;
                    }
                }

                var saveResult = await SaveRecordAsync();
                if (saveResult.Successful)
                {
                    //See notes on this method we allow the view model to override what happens when the record is saved.
                    HasDirtyChildren = false;
                    NotifyParentChildDirty();

                    await PostSaveAsync();
                }
            }
        }

        /* By default when the view model is saved we simply close the page, the view model can overide this to provide different behavior*/
        public virtual Task PostSaveAsync()
        {
            return ViewModelNavigation.GoBackAsync();
        }

        private void Form_OptionSelected(object sender, OptionSelectedEventArgs e)
        {
            OptionSelected(e.Key, e.Value);
            FormAdapter.Refresh();
        }

        protected virtual void OptionSelected(string name, string value) { }


        protected abstract String GetRequestUri();


        protected abstract void BuildForm(EditFormAdapter form);


        public override Task InitAsync()
        {
            return PerformNetworkOperation(LoadView);
        }

        public void ShowRow(string name)
        {
            View[name.ToFieldKey()].IsVisible = true;
        }

        public void HideRow(string name)
        {
            View[name.ToFieldKey()].IsVisible = false;
        }

        public void SetValue(string name, string value)
        {
            View[name.ToFieldKey()].Value = value;
        }

        private bool _hasDirtyChildren;
        public bool HasDirtyChildren
        {
            get { return _hasDirtyChildren; }
            set
            {
                Set(ref _hasDirtyChildren, value);
            }
        }

        public void NotifyParentChildDirty()
        {
            HasDirtyChildren = true;
            if (LaunchArgs.ParentViewModel is IFormParentViewModel)
            {
                (LaunchArgs.ParentViewModel as IFormParentViewModel).NotifyParentChildDirty();
            }

            if (LaunchArgs.ParentViewModel is IListViewModel)
            {
                (LaunchArgs.ParentViewModel as IListViewModel).MarkAsShouldRefresh();
            }
        }
    }


    public interface IFormParentViewModel
    {
        bool HasDirtyChildren { get; set; }
        void NotifyParentChildDirty();
    }
}
