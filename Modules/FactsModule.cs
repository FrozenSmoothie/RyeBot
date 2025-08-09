using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using RyeBot.Builder;

namespace RyeBot.Modules
{
    public class FactsModule : InteractionModuleBase<ShardedInteractionContext>
    {
        private static readonly ConcurrentDictionary<ulong, CancellationTokenSource> _factSenders = new();

        private static readonly string[] Facts =
{
    // Original 10
    "A group of flamingos is called a flamboyance.",
    "The unicorn is the national animal of Scotland.",
    "Honey never spoils.",
    "A shrimp's heart is in its head.",
    "It is impossible for most people to lick their own elbow.",
    "A crocodile cannot stick its tongue out.",
    "The national animal of Australia is the red kangaroo.",
    "The oldest known living tree is a bristlecone pine named Methuselah, which is over 4,800 years old.",
    "A group of owls is called a parliament.",
    "The Eiffel Tower can be 15 cm taller during the summer.",

    // Expanded (mix of simple, fun, silly, educational; some playful/exaggerated marked with (Playful))
    "Octopuses have three hearts.",
    "Bananas are berries, but strawberries are not real botanical berries.",
    "Koalas have fingerprints almost identical to human fingerprints.",
    "Sea otters hold hands while sleeping so they don’t drift apart.",
    "Wombat poo is cube-shaped.",
    "Goats have rectangular pupils.",
    "Butterflies taste with their feet.",
    "Cats can’t taste sweetness very well.",
    "Baby goats are called kids.",
    "Male seahorses carry the babies.",
    "Sharks are older than trees.",
    "Tardigrades can survive extreme conditions that would kill most life.",
    "The Moon causes most of Earth’s tides.",
    "You can’t hum easily while holding your nose.",
    "Hiccups are little spasms of your diaphragm.",
    "An octopus can squeeze through any gap bigger than its beak.",
    "Sloths move so slowly that algae can grow on their fur.",
    "A blue whale’s heart can weigh as much as a small car.",
    "Goosebumps are a leftover reaction from when humans had more body hair.",
    "A group of crows is called a murder.",
    "A group of ravens is called an unkindness.",
    "Cows have best friends.",
    "Some ants farm aphids for sugary honeydew.",
    "Termites and ants help recycle dead plant material.",
    "Mosquitoes are attracted to the carbon dioxide you breathe out.",
    "Octopus blood is blue because it uses copper for oxygen transport.",
    "Penguins look like they’re wearing tuxedos because of countershading camouflage.",
    "Dolphins name themselves with signature whistles.",
    "Honeybees can recognize human faces.",
    "Some jellyfish can revert to a younger stage after adulthood.",
    "Horses can sleep standing up, but need to lie down for deep sleep.",
    "Ostriches have eyes bigger than their brains.",
    "A sneeze can travel fast like a tiny air burst.",
    "Spiders sometimes ‘balloon’ by using silk to catch the wind.",
    "There are more stars in the observable universe than grains of sand on Earth’s beaches.",
    "A day on Venus is longer than a year on Venus.",
    "Saturn would float in water (if you had a giant tub).",
    "Jupiter’s Great Red Spot is a giant storm.",
    "Mars appears red because of iron oxide (rust) dust.",
    "Lightning is hotter than the surface of the Sun.",
    "Hot water can sometimes freeze faster than cold water (Mpemba effect).",
    "Polar bear fur is transparent, not white.",
    "Carrots used to be more commonly purple than orange.",
    "Apples float in water because they are about one quarter air.",
    "Peanuts are legumes, not true nuts.",
    "Avocados are berries.",
    "Hummingbirds can flap their wings more than 50 times a second.",
    "Some parrots can learn hundreds of words.",
    "Ravens can solve multi-step puzzles.",
    "Squirrels sometimes forget buried nuts and help plant trees.",
    "Butterflies start out as caterpillars that basically liquefy inside a chrysalis.",
    "Starfish can regrow lost arms.",
    "Some lizards can detach their tails to escape predators.",
    "Certain frogs can survive being frozen and thaw out.",
    "Camels store fat (not water) in their humps.",
    "Bats are the only mammals capable of sustained flight.",
    "A giraffe’s neck has the same number of vertebrae as a human neck (seven).",
    "Your tongue print is unique, like your fingerprint.",
    "Your nose can detect more smells than we can easily name.",
    "The smallest bone in the human body is the stapes in the ear.",
    "Bones are stronger than concrete by weight.",
    "Humans share about 60% of their DNA with bananas.",
    "Your skin is your largest organ.",
    "Yawns can be contagious even if you only read about them.",
    "Octopus arms can react after being detached for a short time.",
    "Jellyfish have existed longer than dinosaurs.",
    "Some snakes can sense heat with special pits near their noses.",
    "Crickets chirp faster in warmer temperatures.",
    "Antarctica is technically the world’s largest desert.",
    "Some deserts get less than one centimeter of rain a year.",
    "A rainbow is actually a full circle; we usually see only part of it.",
    "Fog is a low cloud.",
    "Clouds can weigh many tons.",
    "The smell of rain on dry ground is called petrichor.",
    "Wind is air moving from high pressure to low pressure.",
    "Snowflakes are crystals of ice that form around tiny particles.",
    "Leaves change color in autumn when chlorophyll breaks down.",
    "Trees form growth rings that can tell their age.",
    "Bamboo can grow astonishingly fast.",
    "Mosses can dry out and then revive with moisture.",
    "Ferns reproduce with spores instead of seeds.",
    "Seeds contain a tiny plant and stored food.",
    "Bees are important pollinators for many crops.",
    "Some plants can move slowly toward light sources.",
    "Sunflowers can track the Sun (heliotropism) while growing.",
    "Venus is the hottest planet in our solar system, not Mercury.",
    "Neptune has extremely fast winds.",
    "Comets are like dirty snowballs.",
    "Asteroids are mostly rock or metal, smaller than planets.",
    "Pluto is a dwarf planet in the Kuiper Belt.",
    "Some spacecraft travel faster than 28,000 km/h.",
    "GPS satellites must account for time differences due to relativity.",
    "Time passes slightly faster on a mountain than at sea level.",
    "Humans glow faintly in visible light, but it’s too weak to see.",
    "The International Space Station orbits Earth roughly every 90 minutes.",
    "Black holes are regions where gravity is so strong not even light escapes.",
    "Neutron stars are incredibly dense; a teaspoon would weigh billions of tons.",
    "The Sun is a star made mostly of hydrogen and helium.",
    "Solar eclipses happen when the Moon blocks the Sun.",
    "Lunar eclipses occur when Earth blocks sunlight from the Moon.",
    "Tides are strongest during full and new moons (spring tides).",
    "A blue moon is the second full moon in a calendar month.",
    "Some fish can change gender if needed.",
    "Clownfish start male and some can become female.",
    "Lobsters taste with their legs and chew with their stomachs.",
    "Crabs walk sideways because of the way their legs bend.",
    "Shrimp can snap a claw fast enough to make a small bubble shockwave.",
    "Electric eels can generate strong electric shocks.",
    "Seahorses swim upright and are poor swimmers compared to many fish.",
    "Manatees are sometimes called sea cows.",
    "A group of porcupines is called a prickle.",
    "A group of ferrets is called a business.",
    "A group of jellyfish can be called a smack.",
    "A group of pandas can be called an embarrassment (rare usage).",
    "Baby kangaroos are called joeys.",
    "Baby hedgehogs are called hoglets.",
    "Baby rabbits are called kits.",
    "Ducklings can imprint on the first moving thing they see.",
    "Owls can rotate their heads up to about 270 degrees.",
    "Snails can sleep for long periods in harsh conditions.",
    "Earthworms help soil by mixing and aerating it.",
    "Some frogs inflate their bodies to appear bigger.",
    "Toads usually have drier, bumpier skin than frogs.",
    "Male cardinals are bright red; females are more muted for camouflage.",
    "Caterpillars can eat many times their body weight.",
    "Spider silk can be stronger than steel by weight.",
    "Flies taste with their feet.",
    "Moths are generally more active at night than butterflies.",
    "Some butterflies migrate long distances, like monarchs.",
    "Geckos can cling to walls using tiny hair-like structures on their toes.",
    "Chameleons change color partly for communication and temperature control.",
    "Platypuses lay eggs even though they are mammals.",
    "Platypuses sense electric signals from prey in the water.",
    "Beavers build dams that create ponds.",
    "Termites build complex mounds with natural ventilation.",
    "Leafcutter ants grow fungus gardens as food.",
    "Some birds mimic sounds like car alarms.",
    "Crows can remember human faces.",
    "Pigeons can be trained to recognize different paintings.",
    "Some parrots live for more than 50 years.",
    "Albatrosses can glide for hours without flapping much.",
    "The Arctic tern migrates farther than most birds each year.",
    "Earth is not a perfect sphere; it bulges at the equator.",
    "Only a small fraction of Earth’s water is fresh and liquid.",
    "Most of the oxygen we breathe comes from tiny ocean plants (phytoplankton).",
    "Coral reefs support a large variety of marine life.",
    "Coral bleaching happens when corals lose their symbiotic algae.",
    "Mangroves protect coastlines from erosion.",
    "Recycling helps reduce the need for some raw materials.",
    "Composting turns food scraps into soil-enriching material.",
    "Glass is an amorphous solid.",
    "Ice floats because it is less dense than liquid water.",
    "Salt lowers the freezing point of water.",
    "Sound travels faster in water than air.",
    "Sound cannot travel through space because there is no air.",
    "Ultraviolet light has more energy than visible light.",
    "Infrared light has longer wavelengths than red visible light.",
    "A prism splits white light into different colors.",
    "Mirages are caused by light bending in layers of air at different temperatures.",
    "Static electricity is a buildup of electric charge.",
    "Magnets have north and south poles.",
    "Electric current is the flow of electrons (in metals).",
    "Batteries store chemical energy and release electrical energy.",
    "A bit is the smallest unit of digital information.",
    "A byte usually has eight bits.",
    "Wi-Fi is a brand name, not an acronym for something complicated.",
    "Turning something off and on can fix simple electronic glitches.",
    "The save icon is often a floppy disk, a technology many people never used.",
    "Dark mode can feel easier on the eyes in low light.",
    "Random number generators in computers are often pseudo-random.",
    "A strong password mixes letters, numbers, and symbols.",
    "Encryption helps keep data private.",
    "Algorithms are just step-by-step instructions.",
    "Emojis evolved from simple text emoticons.",
    "QR codes store data in a grid of black and white modules.",
    "Touchscreens detect where your finger interrupts signals.",
    "Solar panels convert sunlight into electricity.",
    "Wind turbines turn moving air into electrical energy.",
    "Geothermal energy uses Earth’s internal heat.",
    "Hydroelectric dams use moving water to spin turbines.",
    "Comets have tails that point away from the Sun due to solar wind.",
    "Planets orbit stars due to gravity and inertia.",
    "Gravity pulls objects toward each other.",
    "Weight changes with gravity; mass stays the same.",
    "Friction slows motion between surfaces that touch.",
    "Inertia is an object's resistance to changes in motion.",
    "Energy can change forms but is conserved.",
    "Matter is made of atoms.",
    "Atoms are made of protons, neutrons, and electrons.",
    "Electrons orbit in regions we call shells or clouds.",
    "Water expands when it freezes.",
    "Steam is water in gas form.",
    "Dew forms when water vapor condenses on cool surfaces.",
    "Rust forms when iron reacts with oxygen and moisture.",
    "Plants use sunlight to make food in photosynthesis.",
    "Chlorophyll makes most plants look green.",
    "Roots absorb water and nutrients from soil.",
    "Flowers help plants reproduce.",
    "Some seeds travel by wind with little wings.",
    "Other seeds travel by sticking to animal fur.",
    "Cacti have spines instead of typical leaves to save water.",
    "Desert animals may be active at night to avoid heat.",
    "Camouflage helps animals hide.",
    "Mimicry helps some animals look like something else.",
    "Some animals play dead to avoid predators.",
    "Hibernation helps animals survive winter with less food.",
    "Migration is seasonal movement to better living conditions.",
    "Whales communicate with low sounds over long distances.",
    "Dolphins use echolocation to understand their surroundings.",
    "Bubbles can form rainbows due to light interference.",
    "Ice crystals can create halo rings around the Sun or Moon.",
    "Distant mountains can look blue due to air scattering light.",
    "Fire produces light and heat from chemical reactions.",
    "Ash is what’s left after burning mostly non-flammable minerals.",
    "A fossil is a trace or remains of ancient life preserved in rock.",
    "Amber is fossilized tree resin.",
    "Coal formed from ancient plant material.",
    "Oil and gas formed from ancient tiny marine life compressed over time.",
    "Diamonds form under high pressure and temperature deep underground.",
    "Opals can diffract light and show rainbow colors.",
    "Sand can be made from weathered rocks, shells, or even volcanic glass.",
    "Soil layers are called horizons.",
    "Earthquakes happen when underground rock suddenly shifts.",
    "Volcanoes release molten rock called lava when it reaches the surface.",
    "Tsunamis are large ocean waves often caused by underwater quakes.",
    "A compass needle points toward Earth’s magnetic poles.",
    "Satellites help with navigation, weather, and communication.",
    "A constellation is a pattern people see in the stars.",
    "The Milky Way is the galaxy we live in.",
    "Galaxies can collide over millions of years.",
    "Some stars end their lives in huge explosions called supernovae.",
    "The Northern Lights are caused by charged particles interacting with Earth’s atmosphere.",
    "A light-year is the distance light travels in one year.",
    "Rainbows appear opposite the Sun in the sky.",
    "Double rainbows show reversed colors in the second arc.",
    "Dew point is the temperature where air becomes saturated with moisture.",
    "Humidity is the amount of water vapor in the air.",
    "A thermometer measures temperature.",
    "A barometer measures air pressure.",
    "High pressure often brings clearer skies.",
    "Low pressure can bring clouds or storms.",
    "Tornadoes are rotating columns of air from severe storms.",
    "Hail forms when updrafts keep ice aloft to add layers.",
    "Lightning forms when charge builds up in storm clouds.",
    "Thunder is the sound from heated air expanding after lightning.",
    "Solar panels work best when pointed toward the Sun.",
    "Reusing items can reduce waste.",
    "Turning off lights saves energy.",
    "Short showers can save water.",
    "A compost pile needs air, moisture, and a balance of greens and browns.",
    "Paper can often be recycled several times.",
    "Glass can be recycled many times without losing purity.",
    "Plastic types are marked with numbers for sorting.",
    "Metal can be melted and reused.",
    "Some algae blooms can harm fish by reducing oxygen.",
    "Oxygen in water helps fish and aquatic life breathe.",
    "Fish use gills to extract oxygen from water.",
    "Some animals have armor-like shells for protection.",
    "Claws and teeth often show what an animal eats.",
    "Herbivores usually have flatter teeth for grinding plants.",
    "Carnivores often have sharper teeth for cutting meat.",
    "Omnivores eat both plants and animals.",
    "Some birds have hollow bones to help them fly.",
    "Feathers can provide insulation as well as flight.",
    "Scales protect reptiles and fish.",
    "Insects have six legs.",
    "Spiders have eight legs.",
    "Crustaceans like crabs and shrimp have hard exoskeletons.",
    "Insects breathe through tiny holes called spiracles.",
    "Fireflies use light to attract mates.",
    "Some mushrooms glow faintly in the dark.",
    "Mold is a type of fungus.",
    "Yeast makes bread rise by producing gas.",
    "Cheese forms when milk proteins curdle.",
    "Popcorn pops when trapped moisture turns to steam.",
    "Spicy food feels hot due to a chemical trick on nerves.",
    "Chocolate melts near body temperature for a smooth feel.",
    "Ice cream headaches happen when something cold hits the roof of your mouth.",
    "An orange is a hybrid of older citrus varieties.",
    "Watermelons are mostly water by weight.",
    "Lemons float but limes may sink because of density differences.",
    "Soda fizzes from dissolved carbon dioxide gas.",
    "Bread crust forms from heat changing the outer dough.",
    "Some people can fold their tongue; others cannot.",
    "Some people can wiggle their ears.",
    "Your dominant hand is usually stronger.",
    "Sneezing helps clear irritants from the nose.",
    "Blinking helps keep your eyes moist and clear.",
    "Tears help protect and clean the eyes.",
    "Hair and nails are made mostly of keratin.",
    "Taste buds can detect sweet, salty, sour, bitter, and umami.",
    "Your heart pumps blood through vessels all around your body.",
    "Red blood cells carry oxygen.",
    "White blood cells help fight germs.",
    "Platelets help blood clot when you’re cut.",
    "Your skeleton supports and protects organs.",
    "Muscles pull on bones to move your body.",
    "Nerves carry signals between your brain and body.",
    "Brain cells communicate with electrical and chemical signals.",
    "Sleep helps your body rest and repair.",
    "Dreams happen during certain stages of sleep.",
    "Laughing can release feel-good chemicals.",
    "Exercise strengthens muscles and bones.",
    "Drinking water helps your body stay balanced.",
    "Breathing brings in oxygen and removes carbon dioxide.",
    "Your lungs exchange gases with the air you breathe.",
    // Playful / silly extras (still harmless)
    "Cats sometimes knock objects off tables just to see them fall. (Playful)",
    "Squirrels forget where they put some nuts and grow trees accidentally.",
    "Penguins look like they are wearing suits. (Playful)",
    "Goats yelling can sound like people. (Playful)",
    "A cat loaf is when a cat tucks in all four paws. (Playful)",
    "Some dogs sneeze during play to show they are friendly.",
    "Otters sometimes keep a favorite rock.",
    "Capybaras are calm around lots of other animals. (Playful)",
    "Owls have feathers shaped to fly more quietly.",
    "Parrots can dance to music beats. (Playful)",
    "Typing harder does not make the computer go faster. (Playful)",
    "Turning something off and on is often Step 1 in fixing it. (Playful)",
    "Loading bars don’t always show real progress. (Playful)",
    "Dark mode can feel cooler and more stylish. (Playful)",
    "This fact is a placeholder to let you breathe. (Playful)",
    "If you smiled reading these, mission accomplished. (Playful)"
};

        private static readonly Random Rng = new();
        private readonly EmbedTemplateBuilder _embedTemplateBuilder;

        public FactsModule(EmbedTemplateBuilder embedTemplateBuilder)
        {
            _embedTemplateBuilder = embedTemplateBuilder;
        }

        private Embed BuildFactEmbed(string fact)
        {
            return _embedTemplateBuilder.BuildEmbed(
                title: "Random Fact",
                description: fact,
                color: Color.Blue).Build();
        }

        [SlashCommand("fact", "Get a random fact.")]
        public async Task GetRandomFact()
        {
            var fact = Facts[Rng.Next(Facts.Length)];
            var embed = BuildFactEmbed(fact);
            await RespondAsync(embed: embed);
        }

        [SlashCommand("start-facts", "Starts sending facts to this channel every 4 hours.")]
        public async Task StartFacts()
        {
            var cts = new CancellationTokenSource();
            if (_factSenders.TryAdd(Context.Channel.Id, cts))
            {
                _ = Task.Run(async () =>
                {
                    while (!cts.Token.IsCancellationRequested)
                    {
                        await Task.Delay(TimeSpan.FromHours(4), cts.Token);
                        if (cts.Token.IsCancellationRequested) break;

                        var fact = Facts[Rng.Next(Facts.Length)];
                        var embed = BuildFactEmbed(fact);
                        await Context.Channel.SendMessageAsync(embed: embed);
                    }
                }, cts.Token);

                await RespondAsync("Started sending facts every 4 hours.", ephemeral: true);
            }
            else
            {
                await RespondAsync("Facts are already being sent to this channel.", ephemeral: true);
            }
        }

        [SlashCommand("stop-facts", "Stops sending facts to this channel.")]
        public async Task StopFacts()
        {
            if (_factSenders.TryRemove(Context.Channel.Id, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
                await RespondAsync("Stopped sending facts to this channel.", ephemeral: true);
            }
            else
            {
                await RespondAsync("Facts are not currently being sent to this channel.", ephemeral: true);
            }
        }
    }
}