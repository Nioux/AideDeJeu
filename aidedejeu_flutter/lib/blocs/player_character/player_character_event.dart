import 'package:aidedejeu_flutter/models/items.dart';
import 'package:equatable/equatable.dart';

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

