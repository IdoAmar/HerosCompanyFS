import { ability } from "./AbilityType.model";

export interface HeroDTO{
    HeroId: string,
    Name: string,
    Ability: ability,
    StartedAt: Date,
    SuitColors: string,
    StartingPower: number,
    CurrentPower: number,
    Trainable: boolean
}