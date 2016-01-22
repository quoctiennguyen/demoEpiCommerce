EPiServer.Commerce


Installation
============

During installation of this nuget package, any existing files and directory have been overwritten with new ones.

Update search index folder
==========================

If you are upgrading an existing Commerce site, this step can be skipped.
This package installation sets the search index folder to [appDataPath]\Search\ECApplication.
It is required for this site and Commerce Manager site to have same search index folder. Either set the
appData/basePath attribute of the episerver.framework config section to the same value for both sites
(this affects both the search index and other configured paths using the [appDataPath] placeholder),
or edit Configs\Mediachase.Search.Config directly and set the basePath attribute of Indexers section
and the storage attribute of LuceneSearchProvider section.