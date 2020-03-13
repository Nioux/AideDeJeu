import 'package:aidedejeu_flutter/models/items.dart';
import 'package:equatable/equatable.dart';

class PlayerCharacterState extends Equatable {

  final RaceItem race;
  final SubRaceItem subRace;
  final List<RaceItem> races;
  final List<SubRaceItem> subRaces;

  final BackgroundItem background;
  final SubBackgroundItem subBackground;
  final List<BackgroundItem> backgrounds;
  final List<SubBackgroundItem> subBackgrounds;

  PlayerCharacterState({
    this.race,
    this.races,
    this.subRace,
    this.subRaces,
    this.background,
    this.backgrounds,
    this.subBackground,
    this.subBackgrounds,
  });

  PlayerCharacterState copyWith({
    RaceItem race,
    List<RaceItem> races,
    SubRaceItem subRace,
    List<SubRaceItem> subRaces,
    BackgroundItem background,
    List<BackgroundItem> backgrounds,
    SubBackgroundItem subBackground,
    List<SubBackgroundItem> subBackgrounds,
  }) {
    return PlayerCharacterState(
      race: race ?? this.race,
      races: races ?? this.races,
      subRace: subRace ?? this.subRace,
      subRaces: subRaces ?? this.subRaces,
      background: background ?? this.background,
      backgrounds: backgrounds ?? this.backgrounds,
      subBackground: subBackground ?? this.subBackground,
      subBackgrounds: subBackgrounds ?? this.subBackgrounds,
    );
  }

  PlayerCharacterState copyWithClean({
    RaceItem race,
    List<RaceItem> races,
    SubRaceItem subRace,
    List<SubRaceItem> subRaces,
    BackgroundItem background,
    List<BackgroundItem> backgrounds,
    SubBackgroundItem subBackground,
    List<SubBackgroundItem> subBackgrounds,
  }) {
    return PlayerCharacterState(
      race: race ?? this.race,
      races: races ?? this.races,
      subRace: race != null ? null : subRace ?? this.subRace,
      subRaces: race != null ? subRaces : subRaces ?? this.subRaces,
      background: background ?? this.background,
      backgrounds: backgrounds ?? this.backgrounds,
      subBackground: background != null ? null : subBackground ?? this.subBackground,
      subBackgrounds: background != null ? subBackgrounds : subBackgrounds ?? this.subBackgrounds,
    );
  }

  @override
  List<Object> get props => [
    race,
    subRace,
    races,
    subRaces,
    background,
    subBackground,
    backgrounds,
    subBackgrounds
  ];
}

