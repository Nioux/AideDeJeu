import 'package:aidedejeu_flutter/blocs/player_character/player_character_event.dart';
import 'package:aidedejeu_flutter/blocs/player_character/player_character_state.dart';
import 'package:aidedejeu_flutter/databases/database.dart';
import 'package:aidedejeu_flutter/databases/database_sqflite.dart';
import 'package:bloc/bloc.dart';

class PlayerCharacterBloc
    extends Bloc<PlayerCharacterEvent, PlayerCharacterState> {
  PlayerCharacterBloc() : super(null) {}

  BaseDB _db = SqfliteDB.instance;

  @override
  PlayerCharacterState get initialState => PlayerCharacterState();

  @override
  Stream<PlayerCharacterState> mapEventToState(
      PlayerCharacterEvent event) async* {
    if (event is RaceEvent) {
      yield* _mapRaceEventToState(event);
    } else if (event is SubRaceEvent) {
      yield* _mapSubRaceEventToState(event);
    } else if (event is OriginEvent) {
      yield* _mapOriginEventToState(event);
    } else if (event is BackgroundEvent) {
      yield* _mapBackgroundEventToState(event);
    } else if (event is SubBackgroundEvent) {
      yield* _mapSubBackgroundEventToState(event);
    } else if (event is LoadEvent) {
      yield* _mapLoadEventToState(event);
    }
  }

  Stream<PlayerCharacterState> _mapRaceEventToState(RaceEvent event) async* {
    var subRaces = await _db.loadSubRaces(event.item);
    yield state.copyWithClean(race: event.item, subRaces: subRaces);
  }

  Stream<PlayerCharacterState> _mapSubRaceEventToState(
      SubRaceEvent event) async* {
    yield state.copyWith(subRace: event.item);
  }

  Stream<PlayerCharacterState> _mapOriginEventToState(
      OriginEvent event) async* {
    yield state.copyWith(origin: event.item);
  }

  Stream<PlayerCharacterState> _mapBackgroundEventToState(
      BackgroundEvent event) async* {
    var subBackgrounds = await _db.loadSubBackgrounds(event.item);
    yield state.copyWithClean(
        background: event.item, subBackgrounds: subBackgrounds);
  }

  Stream<PlayerCharacterState> _mapSubBackgroundEventToState(
      SubBackgroundEvent event) async* {
    yield state.copyWith(subBackground: event.item);
  }

  Stream<PlayerCharacterState> _mapLoadEventToState(LoadEvent event) async* {
    var races = await _db.loadRaces();
    var origins = await _db.loadOrigins();
    var backgrounds = await _db.loadBackgrounds();
    yield state.copyWith(
        races: races, origins: origins, backgrounds: backgrounds); // state;
  }
}
