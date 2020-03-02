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

  String get app_title {
    return Intl.message(
      'Hello World',
      name: 'app_title',
      desc: 'Title for the application',
      locale: localeName,
    );
  }

  String get library_title {
    return Intl.message(
      'Library',
      name: 'library_title',
      desc: 'Title for the Library page',
      locale: localeName,
    );
  }

  String get pceditor_title {
    return Intl.message(
      'Player Characters',
      name: 'pceditor_title',
      desc: 'Title for the Player Characters page',
      locale: localeName,
    );
  }

  String get about_title {
    return Intl.message(
      'About...',
      name: 'about_title',
      desc: 'Title for the About page',
      locale: localeName,
    );
  }

  String get bookmarks_title {
    return Intl.message(
      'Bookmarks',
      name: 'bookmarks_title',
      desc: 'Title for the Bookmarks page',
      locale: localeName,
    );
  }

  String get search_title {
    return Intl.message(
      'Search',
      name: 'search_title',
      desc: 'Title for the Search page',
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
