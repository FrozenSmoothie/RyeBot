# function for loading packages
using <- function(...) {
    libs <- unlist(list(...))
    req <- unlist(lapply(libs, require, character.only = TRUE))
    need <- libs[req == FALSE]
    if (length(need) > 0) { 
        install.packages(need)
        lapply(need, require, character.only = TRUE)
    }
}

# get current file location
getCurrentFileLocation <-  function()
{
    this_file <- commandArgs() %>% 
    tibble::enframe(name = NULL) %>%
    tidyr::separate(col=value, into=c("key", "value"), sep="=", fill='right') %>%
    dplyr::filter(key == "--file") %>%
    dplyr::pull(value)
    if (length(this_file)==0)
    {
      this_file <- rstudioapi::getSourceEditorContext()$path
    }
    return(dirname(this_file))
}

# load packages
suppressWarnings(suppressMessages(using("tidyverse", "Rmpfr")))

# loading data
junior2023 <- read.csv(file = paste0(getCurrentFileLocation(), "/../../Data/juniorData/Junior_-_CommandUsageStore2023.csv"))
junior2024 <- read.csv(file = paste0(getCurrentFileLocation(), "/../../Data/juniorData/Junior_-_CommandUsageStore2024.csv"))

# joining data
juniorData <- rbind(junior2023, junior2024)

# selecting columns
juniorData_fil <- juniorData %>% 
  select(
    GuildId,
    Command
  )

# changing to factor
juniorData_fil$Command <- as.factor(juniorData$Command)

# commands per guild weighted
juniorData_bubble <- as_tibble(table(juniorData_fil))

# bubble plot with suppressed messages and warnings
bubblePlotGuild <- ggplot(
  juniorData_bubble, 
  aes(
    x = Command, 
    y = n, 
    color = Command, 
    text = paste("GuildId:", GuildId)
  )) +
  geom_point(alpha = 0.5, size = 1) +
  scale_y_continuous(trans = 'log10') +
  labs(
    title = "Command usage per command",
    subtitle = "Every point represents a guild",
    x = "Commands",
    y = "Count"
  ) +
  theme(legend.position = "none") +
  coord_flip()

# Save the plot in root 
suppressWarnings(
    suppressMessages(
        ggsave(
            paste0(
                getCurrentFileLocation(), 
                "/../../../../../Data/rFigures/juniorCommandUsageGuild.png"
            ), 
            plot = bubblePlotGuild, 
            dpi=300
        )
    )
)

# Save to Debug/Release
suppressWarnings(
    suppressMessages(
        ggsave(
            paste0(
                getCurrentFileLocation(), 
                "/../../Data/rFigures/juniorCommandUsageGuild.png"
            ), 
            plot = bubblePlotGuild, 
            dpi=300
        )
    )
)

# Print console message
message("juniorCommandUsageGuild.png made")

# Clean up
rm(list = ls())