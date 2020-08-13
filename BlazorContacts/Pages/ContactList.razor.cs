using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorContacts.Pages
{
    public partial class ContactList
    {
        private IList<Contact> contacts = new List<Contact>();
        private Contact newContact = new Contact();
        private bool IsLoading = true;

        protected override async Task OnInitializedAsync()
        {
            var randomContact = new Contact()
            {
                FirstName = "Test",
                LastName = "Contact"
            };

            //for (int i = 0; i < 100000; i++)
            //{
            //    contacts.Add(randomContact);
            //}

            contacts = await HttpClient.GetFromJsonAsync<List<Contact>>(Configuration["getContactEndpoint"]);

            IsLoading = false;
        }

        private async Task AddContact()
        {
            if (string.IsNullOrWhiteSpace(newContact.FirstName) && string.IsNullOrWhiteSpace(newContact.LastName))
            {
                return;
            }

            var result = await HttpClient.PostAsJsonAsync<Contact>(Configuration["insertContactEndpoint"], newContact);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var newContactId = await result.Content.ReadAsStringAsync();

                newContact.ContactId = Convert.ToInt32(newContactId);

                contacts.Add(newContact);
            }

            newContact = new Contact();
        }
    }
}
