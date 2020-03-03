import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'l10n/messages_all.dart';

class AppLocalizations {
  AppLocalizations(this.localeName);

  static Future<AppLocalizations> load(Locale locale) {
    final String name =
        locale.countryCode.isEmpty ? locale.languageCode : locale.toString();
    final String localeName = Intl.canonicalizedLocale(name);

    return initializeMessages(localeName).then((_) {
      return AppLocalizations(localeName);
    });
  }

  static AppLocalizations of(BuildContext context) {
    return Localizations.of<AppLocalizations>(context, AppLocalizations);
  }

  final String localeName;

  String get appTitle {
    return Intl.message(
      'Hello World',
      name: 'appTitle',
      desc: 'Title for the application',
      locale: localeName,
    );
  }

  String get libraryTitle {
    return Intl.message(
      'Library',
      name: 'libraryTitle',
      desc: 'Title for the Library page',
      locale: localeName,
    );
  }

  String get pceditorTitle {
    return Intl.message(
      'Player Characters',
      name: 'pceditorTitle',
      desc: 'Title for the Player Characters page',
      locale: localeName,
    );
  }

  String get aboutTitle {
    return Intl.message(
      'About...',
      name: 'aboutTitle',
      desc: 'Title for the About page',
      locale: localeName,
    );
  }

  String get bookmarksTitle {
    return Intl.message(
      'Bookmarks',
      name: 'bookmarksTitle',
      desc: 'Title for the Bookmarks page',
      locale: localeName,
    );
  }

  String get searchTitle {
    return Intl.message(
      'Search',
      name: 'searchTitle',
      desc: 'Title for the Search page',
      locale: localeName,
    );
  }

  String get raceTitle {
    return Intl.message(
      'Race',
      name: 'raceTitle',
      desc: 'Title for the Race page',
      locale: localeName,
    );
  }

  String get backgroundTitle {
    return Intl.message(
      'Background',
      name: 'backgroundTitle',
      desc: 'Title for the Background page',
      locale: localeName,
    );
  }

  String get classTitle {
    return Intl.message(
      'Class',
      name: 'classTitle',
      desc: 'Title for the Class page',
      locale: localeName,
    );
  }

  String get abilitiesTitle {
    return Intl.message(
      'Abilities',
      name: 'abilitiesTitle',
      desc: 'Title for the Abilities page',
      locale: localeName,
    );
  }

  String get othersTitle {
    return Intl.message(
      'Others',
      name: 'othersTitle',
      desc: 'Title for the Others page',
      locale: localeName,
    );
  }


}

class AppLocalizationsDelegate extends LocalizationsDelegate<AppLocalizations> {
  const AppLocalizationsDelegate();

  @override
  bool isSupported(Locale locale) => ['en', 'fr'].contains(locale.languageCode);

  @override
  Future<AppLocalizations> load(Locale locale) => AppLocalizations.load(locale);

  @override
  bool shouldReload(AppLocalizationsDelegate old) => false;
}
