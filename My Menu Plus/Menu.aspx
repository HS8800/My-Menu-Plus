﻿<%@ Page Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="My_Menu_Plus.Menu" %>

<asp:Content ContentPlaceHolderID="pageContent" runat="server">

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Menu</title>

    <%: Styles.Render("~/bundles/MenuStyles") %>


    <link href="https://fonts.googleapis.com/css2?family=Open+Sans:wght@300;400;600;700&display=swap"
          rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@100;200;300;400;500;600;700&display=swap"
          rel="stylesheet" />

</head>

<body>




    <div id="main-scale">
        <div id="main-top">
            <div id="main-content">
                <h1 id="main-title">Name</h1>
                <p id="main-tags">
                    <span>Chinese</span><span>•</span><span>First Tag</span><span>•</span><span>Second Tag</span><span>•</span><span>Deal</span><span>•</span><span>Limited Deal</span>
                </p>
                <p id="main-address">50 Broadcroft Avenue HA7 1PF Stanmore</p>
                <p id="main-description">
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
                    eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim
                    ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut
                    aliquip ex ea commodo consequat. Duis aute irure dolor in
                    reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla
                    pariatur. Excepteur sint occaecat cupidatat non proident, sunt in
                    culpa qui officia deserunt mollit anim id est laborum.
                </p>
                <button id="btn-show">Show More</button>
            </div>
        </div>

    </div>





    <div id="select-main" class="select-disable sticky">
        <div id="select-scroll">
            <div class="select-gap select-arrow-width"></div>
            <div>Appertisers</div>
            <div>Sweet & Sour</div>
            <div>Chicken Dishes</div>
            <div>Pork Dishes</div>
            <div>Duck Dishes</div>
            <div>Beef Dishes</div>
            <div>Noodles</div>
            <div>Specials</div>
            <div>Drinks</div>
            <div class="select-gap select-arrow-width"></div>
        </div>
    </div>



    <div id="scroll-left" class="select-arrow select-arrow-width"></div>
    <div id="scroll-right" class="select-arrow right select-arrow-width"></div>




    <div id="main-container">


        <div id="main-menu">

            <h1>Appertisers</h1>

            <table>
                <tr>
                    <td>
                        <div class="item-name">Item 1</div>
                        <div class="item-description">Hello this is what item 1 is.</div>
                    </td>
                    <td class="item-right">
                        <span class="item-price">£5.99</span><button class="item-btn">+</button>
                    </td>
                </tr>

                <tr>
                    <td>
                        <div class="item-name">Item 1</div>
                        <div class="item-description">Hello this is what item 1 is.</div>
                    </td>
                    <td class="item-right">
                        <span class="item-price">£5.99</span><button class="item-btn">+</button>
                    </td>
                </tr>

                <tr>
                    <td>
                        <div class="item-name">Item 1</div>
                        <div class="item-description">Hello this is what item 1 is.</div>
                    </td>
                    <td class="item-right">
                        <span class="item-price">£5.99</span><button class="item-btn">+</button>
                    </td>
                </tr>

                <tr>
                    <td>
                        <div class="item-name">Item 1</div>
                        <div class="item-description">Hello this is what item 1 is.</div>
                    </td>
                    <td class="item-right">
                        <span class="item-price">£5.99</span><button class="item-btn">+</button>
                    </td>
                </tr>
            </table>

            <h1>Sweet & Sour</h1>

            <table>
                <tr>
                    <td>
                        <div class="item-name">Item 1</div>
                        <div class="item-description">Hello this is what item 1 is.</div>
                    </td>
                    <td class="item-right">
                        <span class="item-price">£5.99</span><button class="item-btn">+</button>
                    </td>
                </tr>

                <tr>
                    <td>
                        <div class="item-name">Item 1</div>
                        <div class="item-description">Hello this is what item 1 is.</div>
                    </td>
                    <td class="item-right">
                        <span class="item-price">£5.99</span><button class="item-btn">+</button>
                    </td>
                </tr>

                <tr>
                    <td>
                        <div class="item-name">Item 1</div>
                        <div class="item-description">Hello this is what item 1 is.</div>
                    </td>
                    <td class="item-right">
                        <span class="item-price">£5.99</span><button class="item-btn">+</button>
                    </td>
                </tr>

                <tr>
                    <td>
                        <div class="item-name">Item 1</div>
                        <div class="item-description">Hello this is what item 1 is.</div>
                    </td>
                    <td class="item-right">
                        <span class="item-price">£5.99</span><button class="item-btn">+</button>
                    </td>
                </tr>
            </table>

        </div>

        <div id="basket-container" class="sticky">
            <div id="basket-list">
                <div id="basket-empty">Your basket is empty</div>
            </div>
            <button class="item-btn basket-btn">Checkout</button>
        </div>
    </div>
</body>

<%: Scripts.Render("~/bundles/MenuScripts") %>
</html>

<</asp:Content>
