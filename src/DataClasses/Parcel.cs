using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoletoNet;

namespace Vivina.Erp.DataClasses
{
    public partial class Parcel
    {
        public Boolean IsRegistered
        {
            get
            {
                return OperationDate.HasValue;
            }
        }

        /// <summary>
        /// Generate the boletu object
        /// </summary>
        /// <returns></returns>
        public BoletoNet.Boleto GenerateBoleto()
        {
            if (FinancierOperation == null)
                throw new ArgumentException("Only can generate Boletu object in Parcel with Payment Method!");

            if (!FinancierOperation.PaymentMethod.IsBoleto)
                throw new ArgumentException("Only can generate Boletu object for Boletu Payment Method!");

            var b = new Boleto(DueDate,
                               Convert.ToDouble(Amount),
                               FinancierOperation.OperationNumber,
                               ParcelId.ToString().PadLeft(13, '0'),
                               FinancierOperation.Account.Agency,
                               FinancierOperation.Account.AccountNumber);

            b.Banco = new Banco(Convert.ToInt32(FinancierOperation.Account.Bank.BankNumber));

            //
            // Empresa
            //
            b.Cedente = new Cedente(
                Company.LegalEntityProfile.CNPJ,
                Company.LegalEntityProfile.CompanyName,
                FinancierOperation.Account.Agency,
                FinancierOperation.Account.AccountNumber,
                FinancierOperation.Account.AccountNumberDigit.ToString());

            //
            // Cliente
            //
            b.Sacado = new Sacado(
                 Invoice.Customer.Identification,
                 Invoice.Customer.Name,
                 new Endereco()
                 {
                     End = Invoice.Customer.Address.Name,
                     Bairro = Invoice.Customer.Address.Neighborhood,
                     Cidade = Invoice.Customer.Address.City,
                     CEP = Invoice.Customer.Address.PostalCode,
                     UF = Invoice.Customer.Address.State
                 }
            );

            return b;
        }

    }
}
