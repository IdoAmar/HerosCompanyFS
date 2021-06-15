import { ability } from "./AbilityType.model";

export interface HeroDTO{
    heroId: string,
    name: string,
    ability: ability,
    startedAt: Date,
    suitColors: string,
    startingPower: number,
    currentPower: number,
    trainable: boolean
}