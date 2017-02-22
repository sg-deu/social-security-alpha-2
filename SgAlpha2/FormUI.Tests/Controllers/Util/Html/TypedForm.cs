using System;
using System.Collections.Generic;
using System.Linq;
using FormUI.Tests.Controllers.Util.Http;

namespace FormUI.Tests.Controllers.Util.Html
{
    public class TypedForm<T>
    {
        protected   ElementWrapper      _element;
        protected   string              _method;
        protected   string              _action;
        protected   IList<FormValue>    _formValues     = new List<FormValue>();
        protected   IList<SubmitValue>  _submitValues   = new List<SubmitValue>();

        public TypedForm(ElementWrapper element = null, string method = "", string action = "")
        {
            _element = element;
            SetMethod(method);
            SetAction(action);
        }

        public ElementWrapper           Element         { get { return _element; } }
        public string                   Method          { get { return _method; } }
        public string                   Action          { get { return _action; } }
        public IEnumerable<FormValue>   FormValues      { get { return _formValues; } }
        public IEnumerable<SubmitValue> SubmitValues    { get { return _submitValues; } }

        public TypedForm<T> SetMethod(string method)
        {
            _method = (method ?? "").ToLower() == "post" ? "post" : "get";
            return this;
        }

        public TypedForm<T> SetAction(string action)
        {
            _action = action ?? "";
            return this;
        }

        public TypedForm<T> AddFormValue(FormValue formValue)
        {
            _formValues.Add(formValue);
            return this;
        }

        public TypedForm<T> AddSubmitValue(SubmitValue submitValue)
        {
            _submitValues.Add(submitValue);
            return this;
        }

        public FormValue[] Get(string name)
        {
            return _formValues.Where(fv => fv.Name == name).ToArray();
        }

        public FormValue GetSingle(string name)
        {
            var formValues = Get(name);

            if (formValues.Length == 0)
                throw new Exception(string.Format("Could not find entry '{0}' in form values", name));

            if (formValues.Length > 1)
                throw new Exception(string.Format("Found multiple form values for '{0}'", name));

            return formValues.Single();
        }

        public TypedForm<T> AddFormValues(Request request)
        {
            request.StartForm();

            foreach (var formValue in _formValues)
                formValue.AddFormValue(request);

            return this;
        }

        public Response Submit(ISimulatedHttpClient client, Action<Request> modifier = null)
        {
            var submit = SingleSubmit("Could not find single submit", _submitValues);
            return Submit(submit, client, modifier);
        }

        public Response SubmitValue(string value, ISimulatedHttpClient client, Action<Request> modifier = null)
        {
            var submitsWithValue = _submitValues.Where(sv => sv.Value == value).ToList();
            var submit = SingleSubmit("Could not find submit with value " + value, submitsWithValue);
            return Submit(submit, client, modifier);
        }

        public Response SubmitName(string name, ISimulatedHttpClient client, Action<Request> modifier = null)
        {
            var submitsWithName = _submitValues.Where(sv => sv.Name == name).ToList();
            var submit = SingleSubmit("Could not find submit with name " + name, submitsWithName);
            return Submit(submit, client, modifier);
        }

        public Response SubmitNameValue(string name, string value, ISimulatedHttpClient client, Action<Request> modifier = null)
        {
            var submits = _submitValues.Where(sv => sv.Name == name && sv.Value == value).ToList();
            var submit = SingleSubmit("Could not find submit with name " + name + " and value " + value, submits);
            return Submit(submit, client, modifier);
        }

        public Response Submit(SubmitValue submit, ISimulatedHttpClient client, Action<Request> modifier = null)
        {
            var request = new Request(Action, Method);
            AddFormValues(request);

            if (submit != null)
                submit.SetFormValue(request);

            return client.Process(request, modifier);
        }

        private SubmitValue SingleSubmit(string expected, IList<SubmitValue> submits)
        {
            if (submits.Count != 1)
                throw new Exception(string.Format("{0}: count={1} {2}", expected, _submitValues.Count, string.Join(", ", _submitValues.Select(sv => string.Format("({0}={1})", sv.Name, sv.Value)))));

            return submits[0];
        }
    }
}
