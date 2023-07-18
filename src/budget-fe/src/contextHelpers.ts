import {Context, useContext} from "react";

export function useContextNullGuarded<TContextProps>(ctx: Context<TContextProps | null>) {
    const contextOrNull = useContext(ctx);
    if (contextOrNull == null)
        throw new Error(`Context cannot be null!`)
    
    return contextOrNull;
}