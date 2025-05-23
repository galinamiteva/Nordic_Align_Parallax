﻿using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace Nordic_Align_V7.ViewModels;

public class AddressComponent : IComponent
{
    private readonly string _name;
    private readonly string _street;
    private readonly string _city;
    private readonly string _country;
    private readonly string _postalCode;
    private readonly string _email;

    public AddressComponent( string name, string street, string city, string country,
                             string postalCode, string email)
    {
        _name = name;
        _street = street;
        _city = city;
        _country = country;
        _postalCode = postalCode;
        _email = email;
       // _phone = phone;
    }

    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(2);
            column.Item().PaddingBottom(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
            column.Item().Text(_name);
            column.Item().Text(_street);
            column.Item().Text($"{_postalCode}, {_city}");
            column.Item().Text(_country);
            column.Item().Text($"Email: {_email}");
           
            column.Item().PaddingBottom(20);
        });
    }
}
