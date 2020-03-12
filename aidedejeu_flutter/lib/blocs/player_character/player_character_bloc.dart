import 'package:aidedejeu_flutter/blocs/player_character/player_character_event.dart';
import 'package:aidedejeu_flutter/blocs/player_character/player_character_state.dart';
import 'package:aidedejeu_flutter/database.dart';
import 'package:aidedejeu_flutter/models/items.dart';
import 'package:bloc/bloc.dart';
import 'package:equatable/equatable.dart';
import 'package:flutter/cupertino.dart';

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
