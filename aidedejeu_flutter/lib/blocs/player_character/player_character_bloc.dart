import 'package:aidedejeu_flutter/database.dart';
import 'package:aidedejeu_flutter/models/items.dart';
import 'package:bloc/bloc.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/cupertino.dart';

class PlayerCharacterState extends Equatable {
  final BuildContext context;

  final RaceItem race;
  final SubRaceItem subRace;
  final List<RaceItem> races;
  final List<SubRaceItem> subRaces;

  final BackgroundItem background;
  final SubBackgroundItem subBackground;
  final List<BackgroundItem> backgrounds;
  final List<SubBackgroundItem> subBackgrounds;

  PlayerCharacterState({
    this.context,
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
    BuildContext context,
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
      context: context ?? this.context,
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
    BuildContext context,
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
      context: context ?? this.context,
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

abstract class PlayerCharacterEvent extends Equatable {}

class RaceEvent extends SetItemEvent<RaceItem> {
  RaceEvent(RaceItem item) : super(item);
}

class SubRaceEvent extends SetItemEvent<SubRaceItem> {
  SubRaceEvent(SubRaceItem item) : super(item);
}

class SetItemEvent<T> extends PlayerCharacterEvent {
  final T item;

  @override
  List<Object> get props => [item];

  SetItemEvent(T item) : this.item = item;
}

class BackgroundEvent extends SetItemEvent<BackgroundItem> {
  BackgroundEvent(BackgroundItem item) : super(item);
}

class SubBackgroundEvent extends SetItemEvent<SubBackgroundItem> {
  SubBackgroundEvent(SubBackgroundItem item) : super(item);
}

class LoadEvent extends PlayerCharacterEvent {
  @override
  List<Object> get props => [];
}

class PlayerCharacterBloc
    extends Bloc<PlayerCharacterEvent, PlayerCharacterState> {
  BuildContext context;

  PlayerCharacterBloc(BuildContext context) {
    this.context = context;
  }

  @override
  PlayerCharacterState get initialState => PlayerCharacterState(context: context);

  @override
  Stream<PlayerCharacterState> mapEventToState(
      PlayerCharacterEvent event) async* {
    if (event is RaceEvent) {
      yield* _mapRaceEventToState(event);
    } else if (event is SubRaceEvent) {
      yield* _mapSubRaceEventToState(event);
    } else if (event is BackgroundEvent) {
      yield* _mapBackgroundEventToState(event);
    } else if (event is SubBackgroundEvent) {
      yield* _mapSubBackgroundEventToState(event);
    } else if (event is LoadEvent) {
      yield* _mapLoadEventToState(event);
    }

  }
  Stream<PlayerCharacterState> _mapRaceEventToState(
      RaceEvent event) async* {
    var subRaces = await loadSubRaces(context, event.item);
    yield state.copyWithClean(race: event.item, subRaces: subRaces);
  }

  Stream<PlayerCharacterState> _mapSubRaceEventToState(
      SubRaceEvent event) async* {
    yield state.copyWith(subRace: event.item);
  }
  Stream<PlayerCharacterState> _mapBackgroundEventToState(
      BackgroundEvent event) async* {
    var subBackgrounds = await loadSubBackgrounds(context, event.item);
    yield state.copyWithClean(background: event.item,subBackgrounds: subBackgrounds);
  }
  Stream<PlayerCharacterState> _mapSubBackgroundEventToState(
      SubBackgroundEvent event) async* {
    yield state.copyWith(subBackground: event.item);
  }
  Stream<PlayerCharacterState> _mapLoadEventToState(
      LoadEvent event) async* {
    var races = await loadRaces(context);
    var backgrounds = await loadBackgrounds(context);
    yield state.copyWith(races: races, backgrounds: backgrounds); // state;
  }
}
