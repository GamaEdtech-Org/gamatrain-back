namespace GamaEdtech.Common.Identity
{
    using GamaEdtech.Common.Resources;

    using Microsoft.AspNetCore.Identity;

    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string? email)
        {
            var msg = GlobalResource.IdentityError_DuplicateEmail;
            return new()
            {
                Code = nameof(DuplicateEmail),
                Description = string.Format(msg, email),
            };
        }

        public override IdentityError DuplicateUserName(string? userName)
        {
            var msg = GlobalResource.IdentityError_DuplicateUserName;
            return new()
            {
                Code = nameof(DuplicateUserName),
                Description = string.Format(msg, userName),
            };
        }

        public override IdentityError InvalidEmail(string? email)
        {
            var msg = GlobalResource.IdentityError_InvalidEmail;
            return new()
            {
                Code = nameof(InvalidEmail),
                Description = string.Format(msg, email),
            };
        }

        public override IdentityError DuplicateRoleName(string? role)
        {
            var msg = GlobalResource.IdentityError_DuplicateRoleName;
            return new()
            {
                Code = nameof(DuplicateRoleName),
                Description = string.Format(msg, role),
            };
        }

        public override IdentityError InvalidRoleName(string? role)
        {
            var msg = GlobalResource.IdentityError_InvalidRoleName;
            return new()
            {
                Code = nameof(InvalidRoleName),
                Description = string.Format(msg, role),
            };
        }

        public override IdentityError InvalidToken()
        {
            var msg = GlobalResource.IdentityError_InvalidToken;
            return new()
            {
                Code = nameof(InvalidToken),
                Description = msg,
            };
        }

        public override IdentityError InvalidUserName(string? userName)
        {
            var msg = GlobalResource.IdentityError_InvalidUserName;
            return new()
            {
                Code = nameof(InvalidUserName),
                Description = string.Format(msg, userName),
            };
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            var msg = GlobalResource.IdentityError_LoginAlreadyAssociated;
            return new()
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = msg,
            };
        }

        public override IdentityError PasswordMismatch() => new()
        {
            Code = nameof(PasswordMismatch),
            Description = GlobalResource.IdentityError_PasswordMismatch,
        };

        public override IdentityError PasswordRequiresDigit() => new()
        {
            Code = nameof(PasswordRequiresDigit),
            Description = GlobalResource.IdentityError_PasswordRequiresDigit,
        };

        public override IdentityError PasswordRequiresLower() => new()
        {
            Code = nameof(PasswordRequiresLower),
            Description = GlobalResource.IdentityError_PasswordRequiresLower,
        };

        public override IdentityError PasswordRequiresNonAlphanumeric() => new()
        {
            Code = nameof(PasswordRequiresNonAlphanumeric),
            Description = GlobalResource.IdentityError_PasswordRequiresNonAlphanumeric,
        };

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            var msg = GlobalResource.IdentityError_PasswordRequiresUniqueChars;
            return new()
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = string.Format(msg, uniqueChars),
            };
        }

        public override IdentityError PasswordRequiresUpper() => new()
        {
            Code = nameof(PasswordRequiresUpper),
            Description = GlobalResource.IdentityError_PasswordRequiresUpper,
        };

        public override IdentityError PasswordTooShort(int length)
        {
            var msg = GlobalResource.IdentityError_PasswordTooShort;
            return new()
            {
                Code = nameof(PasswordTooShort),
                Description = string.Format(msg, length),
            };
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            var msg = GlobalResource.IdentityError_UserAlreadyHasPassword;
            return new()
            {
                Code = nameof(UserAlreadyHasPassword),
                Description = msg,
            };
        }

        public override IdentityError UserAlreadyInRole(string? role)
        {
            var msg = GlobalResource.IdentityError_UserAlreadyInRole;
            return new()
            {
                Code = nameof(UserAlreadyInRole),
                Description = string.Format(msg, role),
            };
        }

        public override IdentityError UserNotInRole(string? role)
        {
            var msg = GlobalResource.IdentityError_UserNotInRole;
            return new()
            {
                Code = nameof(UserNotInRole),
                Description = string.Format(msg, role),
            };
        }

        public override IdentityError UserLockoutNotEnabled() => new()
        {
            Code = nameof(UserLockoutNotEnabled),
            Description = GlobalResource.IdentityError_UserLockoutNotEnabled,
        };

        public override IdentityError RecoveryCodeRedemptionFailed() => new()
        {
            Code = nameof(RecoveryCodeRedemptionFailed),
            Description = GlobalResource.IdentityError_RecoveryCodeRedemptionFailed,
        };

        public override IdentityError ConcurrencyFailure() => new()
        {
            Code = nameof(ConcurrencyFailure),
            Description = GlobalResource.IdentityError_ConcurrencyFailure,
        };

        public override IdentityError DefaultError() => new()
        {
            Code = nameof(DefaultError),
            Description = GlobalResource.IdentityError_DefaultIdentityError,
        };
    }
}
