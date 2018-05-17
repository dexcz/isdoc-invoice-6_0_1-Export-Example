using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace isdoc_invoice_Export
{
    class Program
    {

       private static List<Invoice> invoices = new List<Invoice>();

        static void Main(string[] args)
        {
            try
            {
                var invoce = new Invoice();

                invoce.DocumentType = DocumentTypeType.Item1;
                invoce.ID = "0";
                invoce.UUID = "0000";
                invoce.IssuingSystem = "EKONOM, účetní a evidenční systém® (ver. 2018/01)";
                invoce.IssueDate = DateTime.Now;
                invoce.TaxPointDate = DateTime.Now;
                invoce.VATApplicable = true;
                invoce.ElectronicPossibilityAgreementReference = new NoteType();
                var note = new NoteType();
                note.Value = "Rozvoz zásilek example";
                invoce.Note = note;
                invoce.LocalCurrencyCode = "CZK";
                invoce.CurrRate = 1;
                invoce.RefCurrRate = 1;

                invoce.AccountingSupplierParty = new AccountingSupplierPartyType();
                invoce.AccountingSupplierParty.Party = new PartyType();

                invoce.AccountingSupplierParty.Party.PartyIdentification = new PartyIdentificationType { UserID = "555555" };
                invoce.AccountingSupplierParty.Party.PartyName = new PartyNameType { Name = "Our firm s.r.o." };
                invoce.AccountingSupplierParty.Party.PostalAddress = new PostalAddressType { StreetName = "Prdelice street", BuildingNumber = "555", CityName = "Prdelice", PostalZone = "111 01", Country = new CountryType { IdentificationCode = "CZ", Name = "Česká republika" } };
                invoce.AccountingSupplierParty.Party.PartyTaxScheme = new PartyTaxSchemeType[] { new PartyTaxSchemeType { CompanyID = "CZ01923111", TaxScheme = "" } };
                invoce.AccountingSupplierParty.Party.Contact = new ContactType { ElectronicMail = "", Name = "", Telephone = "" };
                invoce.AccountingSupplierParty.Party.RegisterIdentification = new RegisterIdentificationType();


                var data2 = new AccountingCustomerPartyType();
                data2.Party = new PartyType();
                data2.Party.PartyIdentification = new PartyIdentificationType { UserID = "" };
                data2.Party.PartyName = new PartyNameType { Name = "Name" };
                data2.Party.PostalAddress = new PostalAddressType { StreetName = "Street", BuildingNumber = "", CityName = "City", PostalZone = "1231313", Country = new CountryType { IdentificationCode = "CZ", Name = "Česká republika" } };
                data2.Party.PartyTaxScheme = new PartyTaxSchemeType[] { new PartyTaxSchemeType { CompanyID = "", TaxScheme = "" } };                

                data2.Party.Contact = new ContactType { ElectronicMail = "our@mail.com", Name = "Name", Telephone = "Tel" };


                invoce.Items = new object[] { data2 };

                invoce.TaxTotal = new TaxTotalType
                {
                    TaxSubTotal = new TaxSubTotalType[] { new TaxSubTotalType { TaxableAmount = 0,
                             TaxAmount = 0,
                             TaxInclusiveAmount = 0,
                             AlreadyClaimedTaxableAmount = 0,
                             AlreadyClaimedTaxAmount = 0,
                             AlreadyClaimedTaxInclusiveAmount = 0,
                             DifferenceTaxableAmount = 0,
                             DifferenceTaxAmount = 0,
                             DifferenceTaxInclusiveAmount = 0,
                             TaxCategory = new TaxCategoryType{  Percent = 15, TaxScheme = "0", VATApplicable = true, LocalReverseChargeFlag = false}

                            }  }
                };

                invoce.LegalMonetaryTotal = new LegalMonetaryTotalType
                {
                    TaxExclusiveAmount = 0,
                    TaxInclusiveAmount = 0,
                    AlreadyClaimedTaxExclusiveAmount = 0,
                    AlreadyClaimedTaxInclusiveAmount = 0,
                    DifferenceTaxExclusiveAmount = 0,
                    DifferenceTaxInclusiveAmount = 0,
                    PayableRoundingAmount = 0,
                    PaidDepositsAmount = 0,
                    PayableAmount = 0
                };


                invoce.PaymentMeans = new PaymentMeansType();

                invoce.PaymentMeans = new PaymentMeansType
                {
                    Payment = new PaymentType[] { new PaymentType { PaidAmount = 0,
                                            PaymentMeansCode = PaymentMeansCodeType.Item42,  Details = new DetailsType{ PaymentDueDate  = DateTime.Now, ID = "0", BankCode  = "0100", Name = "Banka",
                                             IBAN = "", BIC = "",  VariableSymbol = "", ConstantSymbol = "", SpecificSymbol = "", IssueDate = DateTime.Now, DocumentID = ""}  }
                            }

                };

                invoices.Add(invoce);


                if (invoices.Count > 0)
                {   
                    var r = Serialize(invoices);

                    if (r.Contains("<?xml version=\"1.0\" encoding=\"utf-16\"?>"))
                    {
                        r = r.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "<?xml version=\"1.0\"?>");
                    }

                    if (r.Contains("<ArrayOfInvoice xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">"))
                    {
                        r = r.Replace("<ArrayOfInvoice xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "<Invoices>");
                    }

                    if (r.Contains("<Invoice>"))
                    {
                        r = r.Replace("<Invoice>", "<Invoice xmlns=\"http://isdoc.cz/namespace/2013\" version=\"6.0.1\">");
                    }

                    if (r.Contains("</ArrayOfInvoice>"))
                    {
                        r = r.Replace("</ArrayOfInvoice>", "</Invoices>");
                    }


                    Console.WriteLine(r);
                    Console.ReadLine();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong: " + ex.Message);
                Console.ReadLine();
            }
        }


        private static string Serialize(object obj)
        {
            var sb = new StringBuilder();

            var serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(new StringWriter(sb), obj);

            return sb.ToString();
        }
    }
}
