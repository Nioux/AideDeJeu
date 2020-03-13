import 'package:aidedejeu_flutter/blocs/player_character/player_character_bloc.dart';
import 'package:aidedejeu_flutter/blocs/player_character/player_character_event.dart';
import 'package:aidedejeu_flutter/blocs/player_character/player_character_state.dart';
import 'package:aidedejeu_flutter/localization.dart';
import 'package:aidedejeu_flutter/models/items.dart';
import 'package:aidedejeu_flutter/theme.dart';
import 'package:aidedejeu_flutter/widgets/library.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_markdown/flutter_markdown.dart';

class PCEditorPage extends StatelessWidget {

  // widgets generics

  Widget _buildMarkdown(
      BuildContext context, PlayerCharacterState state, String markdown) {
    return MarkdownBody(
      data: markdown ?? "",
      styleSheet: mainMarkdownStyleSheet(context),
      onTapLink: (link) => Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => LibraryPage(id: link)),
      ),
    );
  }

  Widget _buildSubTitle(
      BuildContext context, PlayerCharacterState state, String title) {
    return Text(title,
        style: TextStyle(
          fontSize: 16,
          fontFamily: "Cinzel",
        ));
  }

  Widget _buildItemsWidget<T extends Item>(
      BuildContext context, PlayerCharacterState state,
      {String hintText,
      List<T> items,
      T selectedItem,
      ValueChanged<T> onChanged}) {
    return DropdownButton(
      hint: Text(hintText),
      value: items != null ? selectedItem : "",
      onChanged: (value) {
        onChanged(value);
      },
      items: items != null
          ? items
              .map((e) => DropdownMenuItem(
                    value: e,
                    child: Text(e.name),
                  ))
              .toList()
          : null,
    );
  }

  // widgets specifics

  Widget _buildRacesWidget(BuildContext context, PlayerCharacterState state) {
    return _buildItemsWidget<RaceItem>(
      context,
      state,
      hintText: "Race",
      items: state.races,
      selectedItem: state.race,
      onChanged: (value) {
        //state.setRace(value);
        BlocProvider.of<PlayerCharacterBloc>(context).add(RaceEvent(value));
      },
    );
  }

  Widget _buildSubRacesWidget(
      BuildContext context, PlayerCharacterState state) {
    return state.subRaces != null
        ? _buildItemsWidget<SubRaceItem>(
            context,
            state,
            hintText: "Variante",
            items: state.subRaces,
            selectedItem: state.subRace,
            onChanged: (value) {
              //state.setSubRace(value);
              BlocProvider.of<PlayerCharacterBloc>(context)
                  .add(SubRaceEvent(value));
            },
          )
        : SizedBox.shrink();
  }

  Widget _buildRaceDetailsWidget(
      BuildContext context, PlayerCharacterState state) {
    return state.race != null
        ? Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              _buildSubTitle(context, state,
                  AppLocalizations.of(context).raceAbilityScoreIncrease),
              _buildMarkdown(context, state, state.race?.abilityScoreIncrease),
              _buildMarkdown(
                  context, state, state.subRace?.abilityScoreIncrease),
              Text(""),
              _buildSubTitle(
                  context, state, AppLocalizations.of(context).raceAge),
              _buildMarkdown(context, state, state.race?.age),
              Text(""),
              _buildSubTitle(
                  context, state, AppLocalizations.of(context).raceAlignment),
              _buildMarkdown(context, state, state.race?.alignment),
              Text(""),
              _buildSubTitle(
                  context, state, AppLocalizations.of(context).raceSize),
              _buildMarkdown(context, state, state.race?.size),
              Text(""),
              _buildSubTitle(
                  context, state, AppLocalizations.of(context).raceSpeed),
              _buildMarkdown(context, state, state.race?.speed),
              state.race?.darkvision != null ? Text("") : SizedBox.shrink(),
              state.race?.darkvision != null
                  ? _buildSubTitle(context, state,
                      AppLocalizations.of(context).raceDarkvision)
                  : SizedBox.shrink(),
              state.race?.darkvision != null
                  ? _buildMarkdown(context, state, state.race?.darkvision)
                  : SizedBox.shrink(),
              Text(""),
              _buildSubTitle(
                  context, state, AppLocalizations.of(context).raceLanguages),
              _buildMarkdown(context, state, state.race?.languages),
            ],
          )
        : SizedBox.shrink();
  }

  Widget _buildBackgroundsWidget(
      BuildContext context, PlayerCharacterState state) {
    return _buildItemsWidget<BackgroundItem>(
      context,
      state,
      hintText: "Historique",
      items: state.backgrounds,
      selectedItem: state.background,
      onChanged: (value) {
        //state.setBackground(value);
        BlocProvider.of<PlayerCharacterBloc>(context)
            .add(BackgroundEvent(value));
      },
    );
  }

  Widget _buildSubBackgroundsWidget(
      BuildContext context, PlayerCharacterState state) {
    return state.subBackgrounds != null
        ? _buildItemsWidget<SubBackgroundItem>(
            context,
            state,
            hintText: "Variante",
            items: state.subBackgrounds,
            selectedItem: state.subBackground,
            onChanged: (value) {
              //state.setSubBackground(value);
              BlocProvider.of<PlayerCharacterBloc>(context)
                  .add(SubBackgroundEvent(value));
            },
          )
        : SizedBox.shrink();
  }

  @override
  Widget build(BuildContext context) {
    return MultiBlocProvider(
      providers: [
        BlocProvider<PlayerCharacterBloc>(
          create: (context) {
            return PlayerCharacterBloc()
              ..add(
                LoadEvent(),
              );
          },
        ),
      ],
      child: BlocBuilder<PlayerCharacterBloc, PlayerCharacterState>(
        builder: (context, state) {
          return buildUI(context, state);
        },
      ),
    );
  }

  Widget buildUI(BuildContext context, PlayerCharacterState state) {
    return DefaultTabController(
      length: 5,
      child: Scaffold(
        appBar: AppBar(
          title: Text(AppLocalizations.of(context).pceditorTitle),
          bottom: TabBar(
            labelColor: Colors.black,
            isScrollable: true,
            indicatorColor: Theme.of(context).accentColor,
            // Colors.red,
            indicatorSize: TabBarIndicatorSize.label,
            tabs: <Widget>[
              Text(
                AppLocalizations.of(context).raceTitle,
                style: TextStyle(
                  fontSize: 25,
                  fontFamily: "Cinzel",
                ),
              ),
              Text(
                AppLocalizations.of(context).backgroundTitle,
                style: TextStyle(
                  fontSize: 25,
                  fontFamily: "Cinzel",
                ),
              ),
              Text(
                AppLocalizations.of(context).classTitle,
                style: TextStyle(
                  fontSize: 25,
                  fontFamily: "Cinzel",
                ),
              ),
              Text(
                AppLocalizations.of(context).abilitiesTitle,
                style: TextStyle(
                  fontSize: 25,
                  fontFamily: "Cinzel",
                ),
              ),
              Text(
                AppLocalizations.of(context).othersTitle,
                style: TextStyle(
                  fontSize: 25,
                  fontFamily: "Cinzel",
                ),
              ),
            ],
          ),
        ),
        body: TabBarView(
          children: [
            Container(
              margin: EdgeInsets.all(10.0),
              child: ListView(
                children: <Widget>[
                  _buildRacesWidget(context, state),
                  _buildSubRacesWidget(context, state),
                  _buildRaceDetailsWidget(context, state),
                ],
              ),
            ),
            Container(
              margin: EdgeInsets.all(10.0),
              child: ListView(
                children: <Widget>[
                  _buildBackgroundsWidget(context, state),
                  _buildSubBackgroundsWidget(context, state),
                ],
              ),
            ),
            Container(
              margin: EdgeInsets.all(10.0),
              child: Text(AppLocalizations.of(context).classTitle),
            ),
            Container(
              margin: EdgeInsets.all(10.0),
              child: Text(AppLocalizations.of(context).abilitiesTitle),
            ),
            Container(
              margin: EdgeInsets.all(10.0),
              child: Text(AppLocalizations.of(context).othersTitle),
            ),
          ],
        ),
      ),
    );
  }
}
