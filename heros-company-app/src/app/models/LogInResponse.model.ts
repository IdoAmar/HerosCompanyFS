import { TrainerDTO } from "./TrainerDTO.model";

export interface LogInResponse {
    trainerId : string | undefined,
    trainerUserName : string | undefined,
    authorization: string | null
}