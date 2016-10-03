namespace Lax.Security.Claims.Ldap {

    public interface IUserObjectSidProvider<TUser> {

        string ObjectSidForUser(TUser user);

    }

}