(invoiceString) => {
    var invoice = JSON.parse(invoiceString);
    var invoiceDetailsDOM = document.getElementById('invoice-details');

    // INVOICE DETAILS
    invoiceDetailsDOM.innerHTML = '';
    var invoiceDetails =
        'Nr: <b>' + invoice.Number +
        '</b> <br /> Data wystawienia: <b>' + new Date(invoice.IssueDate).toLocaleDateString('pl-PL') +
        '</b><br /> Termin płatności: <b>' + new Date(invoice.PaymentDate).toLocaleDateString('pl-PL') + '</b>';

    invoiceDetailsDOM.insertAdjacentHTML('beforeend', invoiceDetails);

    // SELLER
    var sellerDOM = document.getElementById('seller');
    sellerDOM.innerHTML = '';
    var seller =
        '<b>Sprzedawca: </b><br />' +
        invoice.Seller.Name + '<br/>' +
        invoice.Seller.AddressLine1 + '<br/>' +
        invoice.Seller.AddressLine2 + '<br/>' +
        'NIP: ' + invoice.Seller.TaxNumber;

    sellerDOM.insertAdjacentHTML('beforeend', seller);

    // BUYER
    var buyerDOM = document.getElementById('buyer');
    buyerDOM.innerHTML = '';
    var buyer = 
        '<b>Nabywca: </b><br />' +
        invoice.Buyer.Name + '<br/>' +
        invoice.Buyer.AddressLine1 + '<br/>' +
        invoice.Buyer.AddressLine2 + '<br/>' +
        'NIP: ' + invoice.Buyer.TaxNumber;

    buyerDOM.insertAdjacentHTML('beforeend', buyer);

    // RECIVER
    var reciverDOM = document.getElementById('reciver');
    reciverDOM.innerHTML = '';
    if (invoice.Reciver.Name !== null) {
        var reciver = 
            '<b>Nabywca: </b><br />' +
            invoice.Reciver.Name + '<br/>' +
            invoice.Reciver.AddressLine1 + '<br/>' +
            invoice.Reciver.AddressLine2 + '<br/>' +
            'NIP: ' + invoice.Reciver.TaxNumber;

        reciverDOM.insertAdjacentHTML('beforeend', reciver);
    }

    // PAYMENT FORM
    var paymentFormDOM = document.getElementById('payment-form');
    paymentFormDOM.innerHTML = '';
    var paymentForm = invoice.PaymentForm === 0 ? 'Przelew bankowy' : 'Gotówka';
    paymentFormDOM.append(paymentForm);

    // PAYMENT DETAILS
    var paymentDetailsDOM = document.getElementById('payment-details');
    paymentDetailsDOM.innerHTML = '';
    if (invoice.PaymentForm === 0) {
        var bankNameDOM = document.getElementById('bank-name');

        bankNameDOM.append(invoice.Bank.Name);
        paymentDetailsDOM.append(invoice.Bank.AccountNumber);
    }

    // ITEMS
    var itemsTableDOM = document.getElementById('items-table');
    for (var item of invoice.Items) {
        var unit = item.Unit === 1 ? 'szt.' : 'kg';
        var trItem =
            '<tr class="item" style="text-align: center;">' +
            '<td>' + item.Order + '</td>' +
            '<td style="text-align: center;">' + item.Name + '</td>' +
            '<td>' + unit + '</td>' +
            '<td>' + item.Quantity + '</td>' +
            '<td>' + item.Price.toFixed(2) + '</td>' +
            '<td>' + item.Value.toFixed(2) + '</td>' +
            '<td>' + item.Tax + '%</td>' +
            '<td>' + item.TaxAmmount.toFixed(2) + '</td>' +
            '<td>' + item.ValueGross.toFixed(2) + '</td>' +
            '</tr>';

        itemsTableDOM.insertAdjacentHTML('beforeend', trItem);
    }

    // TOTAL PRICE
    var totalPriceDOM = document.getElementById('total-price');
    var totalPrice = 'Do zapłaty: <b>' + invoice.AmmountGross + 'PLN </b>';
    totalPriceDOM.insertAdjacentHTML('beforeend', totalPrice);
}