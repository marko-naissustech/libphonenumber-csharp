/*
 * Copyright (C) 2009 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PhoneNumbers.Test
{
    public class TestPhonenumber
    {
        [Fact]
        public void TestEqualSimpleNumber()
        {
            var numberA = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(6502530000L).Build();

            var numberB = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(6502530000L).Build();

            Assert.Equal(numberA, numberB);
            Assert.Equal(numberA.GetHashCode(), numberB.GetHashCode());
        }

        [Fact]
        public void Test_SA_Number()
        {
            var phoneNumbersToValidate = new List<string>() 
            { 
                "+270833077082", "0833077089","+27833077084","+270112781905","0117681906","+27112781907",
                "00270833077082", "(012)7681907", "99270833077082", " (012) 7681908"
            };
            phoneNumbersToValidate.Reverse();

            foreach (string phoneNumber in phoneNumbersToValidate)
            {
                var numberToCheck = phoneNumber;
                if (phoneNumber.StartsWith("00"))
                {
                    numberToCheck = "+" + phoneNumber.Remove(0, 2);
                }
                if (phoneNumber.StartsWith("99"))
                {
                    numberToCheck = "+" + phoneNumber.Remove(0, 2);
                }                               

                var libPhoneNumber = PhoneNumberUtil.GetInstance().Parse(numberToCheck, "ZA");

                Assert.True(PhoneNumberUtil.GetInstance().IsValidNumberForRegion(libPhoneNumber, "ZA"));
            }           
                
        }

        [Fact]
        public void TestEqualWithCountryCodeSourceSet()
        {
            var numberA = new PhoneNumber.Builder()
                .SetRawInput("+1 650 253 00 00").
                SetCountryCodeSource(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN).BuildPartial();
            var numberB = new PhoneNumber.Builder()
                .SetRawInput("+1 650 253 00 00").
                SetCountryCodeSource(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN).BuildPartial();
            Assert.Equal(numberA, numberB);
            Assert.Equal(numberA.GetHashCode(), numberB.GetHashCode());
        }

        [Fact]
        public void TestNonEqualWithItalianLeadingZeroSetToTrue()
        {
            var numberA = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(6502530000L).SetItalianLeadingZero(true).Build();

            var numberB = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(6502530000L).Build();

            Assert.False(numberA.Equals(numberB));
            Assert.False(numberA.GetHashCode() == numberB.GetHashCode());
        }

        [Fact]
        public void TestNonEqualWithDifferingRawInput()
        {
            var numberA = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(6502530000L).SetRawInput("+1 650 253 00 00")
                .SetCountryCodeSource(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN).Build();

            var numberB = new PhoneNumber.Builder()
            // Although these numbers would pass an isNumberMatch test, they are not considered "equal" as
            // objects, since their raw input is different.
                .SetCountryCode(1).SetNationalNumber(6502530000L).SetRawInput("+1-650-253-00-00").
                SetCountryCodeSource(PhoneNumber.Types.CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN).Build();

            Assert.False(numberA.Equals(numberB));
            Assert.False(numberA.GetHashCode() == numberB.GetHashCode());
        }

        [Fact]
        public void TestNonEqualWithPreferredDomesticCarrierCodeSetToDefault()
        {
            var numberA = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(6502530000L).SetPreferredDomesticCarrierCode("").Build();

            var numberB = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(6502530000L).Build();

            Assert.False(numberA.Equals(numberB));
            Assert.False(numberA.GetHashCode() == numberB.GetHashCode());
        }

        [Fact]
        public void TestEqualWithPreferredDomesticCarrierCodeSetToDefault()
        {
            var numberA = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(6502530000L).SetPreferredDomesticCarrierCode("").Build();

            var numberB = new PhoneNumber.Builder()
                .SetCountryCode(1).SetNationalNumber(6502530000L).SetPreferredDomesticCarrierCode("").Build();

            Assert.Equal(numberA, numberB);
            Assert.Equal(numberA.GetHashCode(), numberB.GetHashCode());
        }
    }
}
