import 'package:aidedejeu_flutter/database.dart';
import 'package:aidedejeu_flutter/models/items.dart';
import 'package:bloc/bloc.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/cupertino.dart';

class PlayerCharacterState {
  final BuildContext context;

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
  }) {}

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

  RaceItem race;
  SubRaceItem subRace;
  List<RaceItem> races;
  List<SubRaceItem> subRaces;

  BackgroundItem background;
  SubBackgroundItem subBackground;
  List<BackgroundItem> backgrounds;
  List<SubBackgroundItem> subBackgrounds;

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

  Future<void> initRaces() async {
    print("initRaces");
    var races = await loadRaces(context);
    this.races = races.toList();
  }

  Future<void> initSubRaces(RaceItem race) async {
    print("initSubRaces");
    var subRaces = await loadSubRaces(context, race);
    this.subRaces = subRaces;
  }

  Future<void> initBackgrounds() async {
    print("initBackgrounds");
    var backgrounds = await loadBackgrounds(context);
    this.backgrounds = backgrounds;
  }

  Future<void> initSubBackgrounds(BackgroundItem background) async {
    print("initSubBackgrounds");
    var subBackgrounds = await loadSubBackgrounds(context, background);
    this.subBackgrounds = subBackgrounds;
  }

  // setters

  Future<void> setRace(RaceItem race) async {
    this.race = race;
    this.subRace = null;
    this.subRaces = null;
    await initSubRaces(race);
  }

  Future<void> setSubRace(SubRaceItem subRace) async {
    this.subRace = subRace;
  }

  Future<void> setBackground(BackgroundItem background) async {
    this.background = background;
    this.subBackground = null;
    this.subBackgrounds = null;
    await initSubBackgrounds(background);
  }

  Future<void> setSubBackground(SubBackgroundItem subBackground) async {
    this.subBackground = subBackground;
  }
}

abstract class PlayerCharacterEvent extends Equatable {}

/*
class RacesEvent extends PlayerCharacterEvent {
  List<RaceItem> races;

  @override
  List<Object> get props => [races];
}
*/
class RaceEvent extends PlayerCharacterEvent {
  RaceItem race;

  @override
  List<Object> get props => [race];

  RaceEvent(RaceItem race) {
    this.race = race;
  }
}

class SubRaceEvent extends PlayerCharacterEvent {
  SubRaceItem subRace;

  @override
  List<Object> get props => [subRace];

  SubRaceEvent(SubRaceItem subRace) {
    this.subRace = subRace;
  }
}

class SetItemEvent<T> extends PlayerCharacterEvent {
  T item;

  @override
  List<Object> get props => [item];

  SetItemEvent(T item) {
    this.item = item;
  }
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

//enum PlayerCharacterEvent { setRace, setSubRace, }
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
      final currentState = state.copyWith(); // state;
      await currentState.initRaces();
      await currentState.initBackgrounds();
      yield currentState;
    }

  }
  Stream<PlayerCharacterState> _mapRaceEventToState(
      RaceEvent event) async* {
    final currentState = state.copyWith();
    await currentState.setRace(event.race);
    yield currentState;
  }
  Stream<PlayerCharacterState> _mapSubRaceEventToState(
      SubRaceEvent event) async* {
    final currentState = state.copyWith();
    await currentState.setSubRace(event.subRace);
    yield currentState;
  }
  Stream<PlayerCharacterState> _mapBackgroundEventToState(
      BackgroundEvent event) async* {
    final currentState = state.copyWith();
    await currentState.setBackground(event.item);
    yield currentState;
  }
  Stream<PlayerCharacterState> _mapSubBackgroundEventToState(
      SubBackgroundEvent event) async* {
    final currentState = state.copyWith();
    await currentState.setSubBackground(event.item);
    yield currentState;
  }
}
