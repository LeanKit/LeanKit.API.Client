//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using LeanKit.API.Client.Library.TransferObjects;
using LeanKit.API.Client.Library.Validation;
using NUnit.Framework;
using RestSharp;

namespace LeanKit.API.Client.Library.Tests
{
    [TestFixture]
    public class ValidationServiceTests
    {
        private readonly bool ShowOutput = true;

        private IValidationService _validationService;
        private List<ValidationResult> _validationResults;
        
        [SetUp]
        public void Setup()
        {
            _validationService = new ValidationService(GetSampleBoard());
        }
        
        [TearDown]
        public void TearDown()
        {
            if (ShowOutput)
            {
                foreach (var validationResult in _validationResults)
                {
                    Console.WriteLine(validationResult);
                }
            }
        }

        [Test]
        public void CanValidateDomainIdentity()
        {
            var restRequest = new RestRequest();
            restRequest.AddParameter("laneId", 5, ParameterType.UrlSegment);
            restRequest.AddParameter("tolaneid", 2, ParameterType.UrlSegment);
            restRequest.AddParameter("typeid", -1, ParameterType.UrlSegment);
            restRequest.AddParameter("classofserviceid", 10, ParameterType.UrlSegment);

            _validationResults = _validationService.ValidateRequest(restRequest);
            Assert.IsNotEmpty(_validationResults);
            Assert.AreEqual(4, _validationResults.Count);
        }

        [Test]
        public void CanValidateDomainIdentityWithDifferentCase()
        {
            var restRequest = new RestRequest();
            restRequest.AddParameter("LaNeId", 2, ParameterType.UrlSegment);

            _validationResults = _validationService.ValidateRequest(restRequest);
            Assert.IsEmpty(_validationResults);
        }

        [Test]
        public void CanValidateDomainEntity()
        {
            var restRequest = new RestRequest();
            restRequest.AddParameter("card", new Card
            {
                Id = 1,
                Title = string.Empty,                
                AssignedUserIds = new long[] { 1, 3, 7 }
            }, ParameterType.UrlSegment);

            _validationResults = _validationService.ValidateRequest(restRequest);
            Assert.IsNotEmpty(_validationResults);            
        }

        [Test]
        public void CanValidateNestedProperties()
        {
            var restRequest = new RestRequest();
            restRequest.AddParameter("board", new Board
            {
                Id = 1,
                Title = string.Empty,
                CardTypes = new List<CardType>
                                {
                                    new CardType(),
                                    new CardType {Name = "Test card type"},
                                    new CardType()
                                }
            }, ParameterType.UrlSegment);

            _validationResults = _validationService.ValidateRequest(restRequest);
            Assert.IsNotEmpty(_validationResults);
        }

        private Board GetSampleBoard()
        {
            return new Board
                       {
                           Id = 1,
                           Title = "Sample board",
                           Lanes = new List<Lane>
                                       {
                                           new Lane {Id = 1, Title = "Lane 1"},
                                           new Lane {Id = 2, Title = "Lane 2"},
                                           new Lane {Id = 3, Title = "Lane 3"}
                                       },
                           CardTypes = new List<CardType>
                                       {
                                            new CardType {Id = 1, Name = "Card type 1", IconPath = @"C:\"},
                                            new CardType {Id = 2, Name = "Card type 2", IconPath = @"C:\"},
                                            new CardType {Id = 3, Name = "Card type 3", IconPath = @"C:\"},
                                            new CardType {Id = 4, Name = "Card type 4", IconPath = @"C:\"},
                                       },
                           ClassesOfService = new List<ClassOfService>
                                       {
                                            new ClassOfService {Id = 1, Title = "Class of service 1", IconPath = @"d:\"},
                                            new ClassOfService {Id = 2, Title = "Class of service 2", IconPath = @"d:\"},
                                            new ClassOfService {Id = 3, Title = "Class of service 3", IconPath = @"d:\"},
                                            new ClassOfService {Id = 4, Title = "Class of service 4", IconPath = @"d:\"},
                                       },
                           BoardUsers = new List<User>
                                        {
                                            new User {Id = 1, UserName = "User 1", EmailAddress = "user1@google.com"},
                                            new User {Id = 2, UserName = "User 2", EmailAddress = "user2@google.com"},
                                            new User {Id = 3, UserName = "User 3", EmailAddress = "user3@google.com"},
                                            new User {Id = 4, UserName = "User 4", EmailAddress = "user4@google.com"},
                                        }
                       };
        }
    }
}