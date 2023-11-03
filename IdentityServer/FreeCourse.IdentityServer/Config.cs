﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("resource_catalog"){ Scopes={ "catalog_fullpermission"}},
                new ApiResource("resource_photo_stock"){ Scopes={ "photo_stock_fullpermission"}},
                new ApiResource("resource_basket"){ Scopes={ "basket_fullpermission"}},
                new ApiResource("resource_discount"){ Scopes={ "discount_fullpermission"}},
                new ApiResource("resource_order"){ Scopes={ "order_fullpermission"}},
                new ApiResource("resource_payment"){ Scopes={ "payment_fullpermission"}},
               new ApiResource("resource_gateway"){ Scopes={ "gateway_fullpermission"}},

                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)


            };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){Name="roles",DisplayName="Roles",Description="Kullanıcı rolleri",UserClaims=new[]{"role"} }


                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
               new ApiScope("catalog_fullpermission","Katalog API için full erişim"),
               new ApiScope("photo_stock_fullpermission","Foto API için full erişim"),
                new ApiScope("basket_fullpermission","Sepet için full erişim"),
                 new ApiScope("discount_fullpermission","İnidirim için full erişim"),
                  new ApiScope("order_fullpermission","Sipariş için full erişim"),
                    new ApiScope("payment_fullpermission","ödeme için full erişim"),
                        new ApiScope("gateway_fullpermission","Gateway için full erişim"),
               new ApiScope(IdentityServerConstants.LocalApi.ScopeName)//identity serverdaki controllera istek yaak için 

            };
        //client ASP.NET CORE MVC 
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                //bu client user login gerektirmeyen apiler için 
                new Client
                {  ClientName="Asp.Net Core MVC",
                    ClientId="WebMvcClient",
                    ClientSecrets={new Secret("secret".Sha256()) },
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={ "catalog_fullpermission", "photo_stock_fullpermission", "gateway_fullpermission", IdentityServerConstants.LocalApi.ScopeName }



                },
                  //bu client user login gerektiren apiler için 
                 new Client
                { 
                    ClientName="Asp.Net Core MVC",
                    ClientId="WebMvcClientForUser",
                    AllowOfflineAccess=true,
                    ClientSecrets={new Secret("secret".Sha256()) },
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    AllowedScopes={"basket_fullpermission" ,"discount_fullpermission","order_fullpermission",
                         "payment_fullpermission","gateway_fullpermission",
                         IdentityServerConstants.StandardScopes.Email,IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServerConstants.StandardScopes.Profile,IdentityServerConstants.StandardScopes.OfflineAccess,"roles",
                         IdentityServerConstants.LocalApi.ScopeName },
                    AccessTokenLifetime=1*60*60,   //1 saat
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int) (DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                    RefreshTokenUsage=TokenUsage.ReUse //resfresh token tekrar kullanıalabılır olsun




                }
                 //offline access provides refreshtoken ,elimdeki tokenla bir refreshtoken alırım ,bu olmazsaelimdeki token süresi bitiresekullanıcı tekrra logn ekranına döner


            };
    }
}