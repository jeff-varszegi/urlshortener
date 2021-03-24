
Assumptions and Notes:

1.  All URLs will be treated as non-case-sensitive, and will be stored and returned in lower case.

2.  The character set used for shortenings will be the 36 English alphanumeric characters, and shortenings will be treated as non-case-sensitive.

3.  All shortenings will be computer-generated, i.e. there need be no support for custom URL shortenings supplied by end users. (If so that could be easily supported in future, but would require giving up the one-to-one correspondence of database IDs to external keys implemented here as an efficiency measure. At the time of conversion, all previously used keys would be automatically added to the appropriate table(s).)

It may be useful to add future features such as access control, expirations of registered URLs, etc. later. Hence a separate database primary key would always be kept for each shortening, to allow extensibility with decent performance and sharding capabilities.

4.  New URL manual registrations would be made using a public-facing website or similar, not implemented as part of this demo.

5.  Protection against abuse and malicious attacks would need to be implemented before release. With public-facing APIs which may be used by developers, it is common to provide rate-limited API keys, though this was not dealt with in the design prompt. If the desire were to simply provide the URL shortening service for manual addition of entries through a website, some combination of CAPTCHA on the site, WAF, and not allowing public access to the registration controller methods (by some combination of access control, splitting to a separate controller, etc.) would probably suffice. Likewise, cancellation tokens have not been added yet to this POC.

6.  It is assumed that by runtime a recent-version PostgreSQL instance has been made available to the application, a database has been installed (named "urls") with appropriate access rights, the script in UrlShortener.DataAcess/Scripts has been successfully run on the database using a tool such as PgAdmin, and the database connection string has been added to appsettings.json in the UrlShortener.Api project.

7.  An out-of-process cache would likely be added before first release, but would presumably increase the necessary setup before running the demo. An easy extension point was created for this--just add water in the form of an ICache implementation using Redis or similar, backing the provided in-memory cache. 
