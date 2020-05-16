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

  String translate(name) {
    return Intl.message(
      name,
      name: name,
      desc: name,
      locale: localeName,
    );
  }

  final String localeName;

  String get appTitle {
    return Intl.message(
      'Axes & Dices',
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

  String get originTitle {
    return Intl.message(
      'Origin',
      name: 'originTitle',
      desc: 'Title for the Origin page',
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

  String get raceAbilityScoreIncrease {
    return Intl.message(
      'Ability Score Increase',
      name: 'raceAbilityScoreIncrease',
      desc: '',
      locale: localeName,
    );
  }

  String get raceAge {
    return Intl.message(
      'Age',
      name: 'raceAge',
      desc: '',
      locale: localeName,
    );
  }

  String get raceAlignment {
    return Intl.message(
      'Alignment',
      name: 'raceAlignment',
      desc: '',
      locale: localeName,
    );
  }

  String get raceSize {
    return Intl.message(
      'Size',
      name: 'raceSize',
      desc: '',
      locale: localeName,
    );
  }

  String get raceSpeed {
    return Intl.message(
      'Speed',
      name: 'raceSpeed',
      desc: '',
      locale: localeName,
    );
  }

  String get raceDarkvision {
    return Intl.message(
      'Darkvision',
      name: 'raceDarkvision',
      desc: '',
      locale: localeName,
    );
  }

  String get raceLanguages {
    return Intl.message(
      'Languages',
      name: 'raceLanguages',
      desc: '',
      locale: localeName,
    );
  }

  String get originRegionsOfOrigin {
    return Intl.message(
      'Regions of Origin',
      name: 'originRegionsOfOrigin',
      desc: '',
      locale: localeName,
    );
  }
  String get originMainLanguages {
    return Intl.message(
      'Main Languages',
      name: 'originMainLanguages',
      desc: '',
      locale: localeName,
    );
  }
  String get originAspirations {
    return Intl.message(
      'Aspirations',
      name: 'originAspirations',
      desc: '',
      locale: localeName,
    );
  }
  String get originAvailableSkills {
    return Intl.message(
      'Available Skills',
      name: 'originAvailableSkills',
      desc: '',
      locale: localeName,
    );
  }

  String get monstersTypes {
    return Intl.message(
      'Types',
      name: 'monstersTypes',
      desc: '',
      locale: localeName,
    );
  }

  String get monstersChallenges {
    return Intl.message(
      'Challenges',
      name: 'monstersChallenges',
      desc: '',
      locale: localeName,
    );
  }
  String get monstersSizes {
    return Intl.message(
      'Sizes',
      name: 'monstersSizes',
      desc: '',
      locale: localeName,
    );
  }
  String get monstersSources {
    return Intl.message(
      'Sources',
      name: 'monstersSources',
      desc: '',
      locale: localeName,
    );
  }
  String get monstersTerrains {
    return Intl.message(
      'Terrains',
      name: 'monstersTerrains',
      desc: '',
      locale: localeName,
    );
  }


  String get spellsClasses {
    return Intl.message(
      'Classes',
      name: 'spellsClasses',
      desc: '',
      locale: localeName,
    );
  }

  String get spellsLevels {
    return Intl.message(
      'Levels',
      name: 'spellsLevels',
      desc: '',
      locale: localeName,
    );
  }

  String get spellsSchools {
    return Intl.message(
      'Schools',
      name: 'spellsSchools',
      desc: '',
      locale: localeName,
    );
  }

  String get spellsRituals {
    return Intl.message(
      'Rituals',
      name: 'spellsRituals',
      desc: '',
      locale: localeName,
    );
  }

  String get spellsCastingTimes {
    return Intl.message(
      'Casting Times',
      name: 'spellsCastingTimes',
      desc: '',
      locale: localeName,
    );
  }

  String get spellsRanges {
    return Intl.message(
      'Ranges',
      name: 'spellsRanges',
      desc: '',
      locale: localeName,
    );
  }

  String get spellsVerbalComponents {
    return Intl.message(
      'Verbal Components',
      name: 'spellsVerbalComponents',
      desc: '',
      locale: localeName,
    );
  }

  String get spellsSomaticComponents {
    return Intl.message(
      'Somatic Components',
      name: 'spellsSomaticComponents',
      desc: '',
      locale: localeName,
    );
  }

  String get spellsMaterialComponents {
    return Intl.message(
      'Material Components',
      name: 'spellsMaterialComponents',
      desc: '',
      locale: localeName,
    );
  }

  String get spellsConcentrations {
    return Intl.message(
      'Concentrations',
      name: 'spellsConcentrations',
      desc: '',
      locale: localeName,
    );
  }

  String get spellsDurations {
    return Intl.message(
      'Durations',
      name: 'spellsDurations',
      desc: '',
      locale: localeName,
    );
  }

  String get spellsSources {
    return Intl.message(
      'Sources',
      name: 'spellsSources',
      desc: '',
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
